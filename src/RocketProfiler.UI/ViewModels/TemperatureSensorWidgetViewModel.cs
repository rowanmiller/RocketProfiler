// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Runtime.CompilerServices;
using RocketProfiler.Controller.Hardware;

namespace RocketProfiler.UI.ViewModels
{
    public class TemperatureSensorWidgetViewModel : INotifyPropertyChanged
    {
        private readonly Sensor _sensor;
        private SensorReadEventArgs _lastRead;

        public TemperatureSensorWidgetViewModel(Sensor sensor)
        {
            _sensor = sensor;
            Name = _sensor.Title;
            ThemomoterLabel25 = (int)_sensor.MaxValue * 0.25 + "°C";
            ThemomoterLabel50 = (int)_sensor.MaxValue * 0.5 + "°C";
            ThemomoterLabel75 = (int)_sensor.MaxValue * 0.75 + "°C";
            ThemomoterLabel100 = (int)_sensor.MaxValue + "°C";

            _sensor.SensorReadEvent += (_, e) =>
            {
                _lastRead = e;
                OnPropertyChanged(nameof(CelsiusText));
                OnPropertyChanged(nameof(FarenheightText));
                OnPropertyChanged(nameof(ThermomoterValue));
                OnPropertyChanged(nameof(ErrorMessage));
            };
        }

        public string Name { get; private set; }

        public string ThemomoterLabel25 { get; private set; }
        public string ThemomoterLabel50 { get; private set; }
        public string ThemomoterLabel75 { get; private set; }
        public string ThemomoterLabel100 { get; private set; }

        public string CelsiusText
        {
            get
            {
                var value = _lastRead.Value;
                return value.HasValue
                    ? (int)value + "°C"
                    : string.Empty;
            }
        }

        public string FarenheightText
        {
            get
            {
                var value = _lastRead.Value;
                return value.HasValue
                    ? (int)(value * 9.0 / 5.0 + 32) + "°F"
                    : string.Empty;
            }
        }

        public int ThermomoterValue
        {
            get
            {
                var value = _lastRead.Value;
                return value.HasValue
                    ? (int)(value * 100 / _sensor.MaxValue)
                    : 0;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _lastRead.ErrorMessage;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
