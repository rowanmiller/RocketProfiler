// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OxyPlot;
using OxyPlot.Axes;
using RocketProfiler.Controller.Hardware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RocketProfiler.UI.ViewModels
{
    public class TemperaturePlotViewModel : PlotViewModel, INotifyPropertyChanged
    {
        private static readonly TimeSpan _minimumDuration = TimeSpan.FromMinutes(2.0);

        private readonly Sensor _sensor;
        private readonly List<DataPoint> _dataPoints = new List<DataPoint>();
        private readonly List<DataPoint> _thresholdDataPoints = new List<DataPoint>();
        private  DateTime _startTime;

        public TemperaturePlotViewModel(Sensor sensor)
        {
            _startTime = DateTime.UtcNow;

            _sensor = sensor;

            _sensor.SensorReadEvent += (_, e) =>
            {
                var time = TimeSpanAxis.ToDouble(e.Timestamp - _startTime);

                if (e.IsSuccessfulRead)
                {
                    _dataPoints.Add(new DataPoint(time,e.Value.Value));
                }

                _thresholdDataPoints.Add(new DataPoint(time, _sensor.Threshold));

                OnPropertyChanged(nameof(DataPoints));
                OnPropertyChanged(nameof(ThresholdValues));
            };
        }

        public string SensorName => _sensor.Title;

        public IList<DataPoint> DataPoints => _dataPoints;

        public IList<DataPoint> ThresholdValues => _thresholdDataPoints;

        public override void Restart()
        {
            _dataPoints.Clear();
            _thresholdDataPoints.Clear();
            _startTime = DateTime.UtcNow;

            OnPropertyChanged(nameof(DataPoints));
            OnPropertyChanged(nameof(ThresholdValues));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
