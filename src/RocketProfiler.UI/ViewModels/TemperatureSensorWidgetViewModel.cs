// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RocketProfiler.Controller;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RocketProfiler.UI.ViewModels
{
    public class TemperatureSensorWidgetViewModel : INotifyPropertyChanged
    {
        private readonly Sensor _sensor;

        public TemperatureSensorWidgetViewModel(Sensor sensor)
        {
            _sensor = sensor;

            _sensor.LastRead.PropertyChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(CelsiusText));
                OnPropertyChanged(nameof(FarenheightText));
                OnPropertyChanged(nameof(ThermomoterValue));
            };
        }

        public string CelsiusText => (int)_sensor.LastRead.Value.Value + "°C";

        public string FarenheightText => (int)(_sensor.LastRead.Value.Value * 9.0 / 5.0 + 32) + "°F";

        public int ThermomoterValue => (int)(_sensor.LastRead.Value.Value * 100 / _sensor.MaxValue);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
