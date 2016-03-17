// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RocketProfiler.Controller
{
    public abstract class Sensor : INotifyPropertyChanged
    {
        private SensorValue _value = new SensorValue { Timestamp = DateTime.Now, Value = null };

        protected Sensor(string name, string units, double maxValue)
        {
            Info = new SensorInfo { Name = name, Units = units, MaxValue = maxValue };
        }

        public virtual SensorValue ReadValue() 
            => Value = DoRead();

        public abstract SensorValue DoRead();

        public SensorInfo Info { get; }

        public SensorValue Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
