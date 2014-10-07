﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using loadify.Model;

namespace loadify.ViewModel
{
    public class TrackViewModel : ViewModelBase
    {
        private TrackModel _Track;
        public TrackModel Track
        {
            get { return _Track; }
            set
            {
                if (_Track == value) return;
                _Track = value;
                NotifyOfPropertyChange(() => Track);
            }
        }

        public string Name
        {
            get { return Track.Name; }
            set
            {
                if (Track.Name == value) return;
                Track.Name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public int Duration
        {
            get { return Track.Duration; }
            set
            {
                if (Track.Duration == value) return;
                Track.Duration = value;
                NotifyOfPropertyChange(() => Duration);
            }
        }

        public TrackViewModel(TrackModel track)
        {
            _Track = track;
        }

        public TrackViewModel() :
            this(new TrackModel())
        { }
    }
}
