// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using OxyPlot;
using OxyPlot.Axes;
using RocketProfiler.Controller;

namespace RocketProfiler.UI.ViewModels
{
    public class HistoryPlotViewModel : INotifyPropertyChanged
    {
        private readonly Sensor _sensor;

        private readonly List<DataPoint> _dataPoints = new List<DataPoint>();

        public HistoryPlotViewModel(Sensor sensor)
        {
            _sensor = sensor;
        }

        public void UpdateRun(Run run)
        {
            if (run != null)
            {
                _dataPoints.Clear();

                var startTime = run.StartTime;

                var sensorValues = run
                    .Snapshots
                    .Select(s => s.SensorValues.Single(sv => sv.SensorInfo.Name == _sensor.Info.Name))
                    .ToList();

                _dataPoints.AddRange(
                    sensorValues
                        .Where(v => v.Value.HasValue)
                        .OrderBy(v => v.Timestamp)
                        .Select(v =>
                            new DataPoint(
                                TimeSpanAxis.ToDouble(v.Timestamp - startTime),
                                v.Value.Value)));

                OnPropertyChanged(nameof(DataPoints));
            }
        }

        public IList<DataPoint> DataPoints => _dataPoints;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
