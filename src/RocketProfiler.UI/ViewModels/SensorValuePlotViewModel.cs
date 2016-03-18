// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using OxyPlot;
using OxyPlot.Axes;
using RocketProfiler.Controller;

namespace RocketProfiler.UI.ViewModels
{
    public class SensorValuePlotViewModel : INotifyPropertyChanged
    {
        private static readonly TimeSpan _minimumDuration = TimeSpan.FromMinutes(2.0);

        private readonly SensorInfo _sensorInfo;
        private readonly List<DataPoint> _dataPoints = new List<DataPoint>();
        private readonly List<DataPoint> _thresholdDataPoints = new List<DataPoint>();

        public SensorValuePlotViewModel(SensorInfo sensorInfo)
        {
            _sensorInfo = sensorInfo;
        }

        public void UpdatePlot(IEnumerable<SensorValue> sensorValues)
        {
            _dataPoints.Clear();
            _thresholdDataPoints.Clear();

            var sortedValues = sensorValues
                .Where(v => v.Value.HasValue)
                .OrderBy(v => v.Timestamp)
                .ToList();

            var startTime = sortedValues.First().Timestamp;

            _dataPoints.AddRange(
                sortedValues.Select(v =>
                    new DataPoint(
                        TimeSpanAxis.ToDouble(v.Timestamp - startTime),
                        v.Value.Value)));

            var duration = sortedValues.LastOrDefault()?.Timestamp - startTime;

            _thresholdDataPoints.Add(
                new DataPoint(
                    0.0,
                    Threshold));

            _thresholdDataPoints.Add(
                new DataPoint(
                    TimeSpanAxis.ToDouble(
                        duration == null
                            ? _minimumDuration
                            : duration > _minimumDuration
                                ? duration
                                : _minimumDuration),
                    Threshold));

            OnPropertyChanged(nameof(DataPoints));
            OnPropertyChanged(nameof(ThresholdValues));
        }

        public string SensorName => _sensorInfo.Name;

        public double Threshold => _sensorInfo.Threshold;

        public IList<DataPoint> DataPoints => _dataPoints;

        public IList<DataPoint> ThresholdValues => _thresholdDataPoints;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
