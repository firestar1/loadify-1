﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using loadify.Event;
using loadify.Model;
using loadify.View;
using MahApps.Metro.Controls.Dialogs;
using SpotifySharp;

namespace loadify.ViewModel
{
    public class LoginViewModel : ViewModelBase, IHandle<LoginFailedEvent>, IHandle<LoginSuccessfulEvent>
    {
        private LoadifySession _Session;

        private LoginModel _LoginModel;
        public string Username
        {
            get { return _LoginModel.Username; }
            set
            {
                if (_LoginModel.Username == value) return;
                _LoginModel.Username = value;
                NotifyOfPropertyChange(() => _LoginModel.Username);
            }
        }

        private bool _LoginProcessActive = false;
        public bool LoginProcessActive
        {
            get { return _LoginProcessActive; }
            set
            {
                if (_LoginProcessActive == value) return;
                _LoginProcessActive = value;
                NotifyOfPropertyChange(() => LoginProcessActive);
            }
        }

        public LoginViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) :
            base(eventAggregator, windowManager)
        {
            _LoginModel = new LoginModel();
            _Session = new LoadifySession(_EventAggregator);
        }

        public void OnKeyUp(Key key)
        {
            if (key == Key.Enter)
                Login();
        }

        public void Login()
        {
            LoginProcessActive = true;
            var loginView = GetView() as LoginView;

            // since you can't bind the passwordbox to a property, the viewmodel needs to be aware of the view to access the password entered
            var password = loginView.Password.Password;
            _Session.Login(Username, password);          
        }

        public void Handle(LoginFailedEvent message)
        {
            var view = GetView() as LoginView;
            LoginProcessActive = false;

            switch (message.Error)
            {
                case SpotifyError.BadUsernameOrPassword:
                {
                    view.ShowMessageAsync("Login failed", "Username or password is wrong");
                    break;
                }
                case SpotifyError.UnableToContactServer:
                {
                    view.ShowMessageAsync("Login failed", "No connection to the Spotify servers could be made");
                    break;
                }
                default:
                {
                    view.ShowMessageAsync("Login failed", "Unknown error: " + message.Error);
                    break;
                }
            }
        }

        public void Handle(LoginSuccessfulEvent message)
        {
            var loginView = GetView() as LoginView;
            _WindowManager.ShowWindow(new MainViewModel(_Session));
            loginView.Close();
        }
    }
}
