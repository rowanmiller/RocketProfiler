﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RocketProfiler.Controller
{
    public abstract class CurrentSensorValue : INotifyPropertyChanged
    {
        private SensorValue _value;

        public SensorValue Value
        {
            get { return _value; }
            set
            {
                OnPropertyChanged();
                _value = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
