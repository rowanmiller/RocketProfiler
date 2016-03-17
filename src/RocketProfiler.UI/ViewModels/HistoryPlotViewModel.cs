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
        private readonly List<DataPoint> _dataPoints = new List<DataPoint>();

        public HistoryPlotViewModel(Run run, IEnumerable<SensorValue> sensorValues)
        {
            var startTime = run.StartTime;

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

        public IList<DataPoint> DataPoints => _dataPoints;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
