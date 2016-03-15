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
        private readonly RunController _runController;

        public SensorPlotWidgetViewModel(RunController runController)
        {
            _runController = runController;

            var timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += RedrawGraph;
            timer.Start();
        }

        private void RedrawGraph(object sender, ElapsedEventArgs e)
        {
            DataPoints = _runController
                ?.LastRun
                ?.Snapshots
                ?.Select(s => s.SensorValues.First())
                ?.Select(v =>
                    new DataPoint(
                        DateTimeAxis.ToDouble(v.Timestamp),
                        v.Value)).ToList();

            OnPropertyChanged(nameof(DataPoints));
        }

        public IList<DataPoint> DataPoints { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
