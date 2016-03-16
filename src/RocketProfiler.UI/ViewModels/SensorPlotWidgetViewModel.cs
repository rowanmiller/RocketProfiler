// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using OxyPlot;
using OxyPlot.Axes;
using RocketProfiler.Controller;

namespace RocketProfiler.UI.ViewModels
{
    public class SensorPlotWidgetViewModel : INotifyPropertyChanged
    {
        private readonly Sensor _sensor;
        private readonly RunController _runController;
        private readonly List<DataPoint> _dataPoints;

        public SensorPlotWidgetViewModel(Sensor sensor, RunController runController)
        {
            _sensor = sensor;
            _runController = runController;

            var timer = new Timer();
            timer.Interval = 300;
            timer.Elapsed += RedrawGraph;
            timer.Start();

            _dataPoints = new List<DataPoint>();
        }

        private void RedrawGraph(object sender, ElapsedEventArgs e)
        {
            _dataPoints.Clear();

            _dataPoints.AddRange(
                _runController
                    ?.CurrentRun
                    ?.Snapshots
                    ?.Select(s => s.SensorValues.Single(sv => sv.Sensor.Name == _sensor.Name))
                    ?.Where(v => v.Value.HasValue)
                    ?.Select(v =>
                        new DataPoint(
                            DateTimeAxis.ToDouble(v.Timestamp),
                        v.Value.Value)) ?? new DataPoint[0]);

            OnPropertyChanged(nameof(DataPoints));
        }

        public IList<DataPoint> DataPoints => _dataPoints;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
