// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using RocketProfiler.Controller;
using TimeSpanAxis = OxyPlot.Wpf.TimeSpanAxis;
using System.Linq;
using System.Windows.Media;

namespace RocketProfiler.UI.ViewModels
{
    public class SessionViewModel : INotifyPropertyChanged
    {
        public SessionViewModel(RunRepository runRepository, string title)
        {
            Title = title;
            RunRepository = runRepository;
            Runs = RunRepository.LoadRuns();
        }

        public RunRepository RunRepository { get; }

        public string Title { get; }

        public IList<Run> Runs { get; }

        public Run CurrentRun
        {
            set
            {
                RunRepository.PopulateRun(value);

                var dataSeries = value.Snapshots
                    .SelectMany(s => s.SensorValues)
                    .GroupBy(s => s.SensorInfo);

                PlotWidgets = new List<Plot>();
                foreach (var sensorData in dataSeries)
                {
                    var plotViewModel = new SensorValuePlotViewModel(sensorData.Key);
                    var plot = new Plot { Title = sensorData.Key.Name};

                    plot.Axes.Add(new TimeSpanAxis
                    {
                        Position = AxisPosition.Bottom,
                        StringFormat = "mm:ss"
                    });

                    plot.Series.Add(
                        new LineSeries
                        {
                            ItemsSource = plotViewModel.DataPoints
                        });

                    plot.Series.Add(
                        new LineSeries
                        {
                            ItemsSource = plotViewModel.MaxValues,
                            Color = (Color)ColorConverter.ConvertFromString("Red")
                        });

                    plotViewModel.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == "DataPoints")
                        {
                            Application.Current.Dispatcher.InvokeAsync(() =>
                                plot.InvalidatePlot());
                        }
                    };

                    plotViewModel.UpdatePlot(value.StartTime, sensorData);

                    PlotWidgets.Add(plot);
                }
            }
        }

        public IList<Plot> PlotWidgets { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
