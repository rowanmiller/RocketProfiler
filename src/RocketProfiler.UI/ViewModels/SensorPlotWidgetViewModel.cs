// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
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

        private readonly List<DataPoint> _dataPoints = new List<DataPoint>();

        public SensorPlotWidgetViewModel(Sensor sensor, RunController runController)
        {
            _sensor = sensor;
            _runController = runController;

            var timer = new Timer { Interval = 300 };
            timer.Elapsed += RedrawGraph;
            timer.Start();
        }

        private void RedrawGraph(object sender, ElapsedEventArgs e)
        {
            if (_runController.CurrentRun != null)
            {
                _dataPoints.Clear();

                List<SensorValue> sensorValues;
                DateTime startTime;
                lock (_runController.Lock)
                {
                    startTime = _runController.CurrentRun.StartTime;

                    sensorValues = _runController
                        .CurrentRun
                        .Snapshots
                        .Select(s => s.SensorValues.Single(sv => sv.SensorInfo.Name == _sensor.Info.Name))
                        .ToList();
                }

                _dataPoints.AddRange(
                    sensorValues
                        .Where(v => v.Value.HasValue)
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
