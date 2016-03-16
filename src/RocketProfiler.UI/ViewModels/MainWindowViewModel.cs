// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Windows.Controls;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using RocketProfiler.Controller;
using RocketProfiler.UI.Views;
using System.Timers;
using System.ComponentModel;
using System;
using System.Runtime.CompilerServices;

namespace RocketProfiler.UI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Timer _runTimer;

        public MainWindowViewModel(IEnumerable<Sensor> sensors, RunController runController)
        {
            RunController = runController;

            SensorWidgets = new List<UserControl>();
            PlotWidgets = new List<Plot>();

            foreach (var sensor in sensors)
            {
                SensorWidgets.Add(
                    new TemperatureSensorWidget(
                        new TemperatureSensorWidgetViewModel(sensor)));

                var plotViewModel = new SensorPlotWidgetViewModel(sensor, RunController);

                var plot = new Plot();
                plot.Axes.Add(new OxyPlot.Wpf.TimeSpanAxis
                {
                    Position = AxisPosition.Bottom,
                    StringFormat = "mm:ss"
                });
                plot.Series.Add(
                    new LineSeries
                    {
                        ItemsSource = plotViewModel.DataPoints
                    });

                plotViewModel.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == "DataPoints")
                        {
                            App.Current.Dispatcher.InvokeAsync(() =>
                            plot.InvalidatePlot());
                        }
                    };

                PlotWidgets.Add(plot);
            }

            _runTimer = new Timer(100);
            _runTimer.Elapsed += (_, __) =>
            {
                OnPropertyChanged(nameof(TimerText));
            };
        }

        public void StartRun(string runName, string runDescription)
        {
            RunController.StartRecoding(runName, runDescription);
            _runTimer.Start();
        }

        public void StopRun()
        {
            RunController.StopRecording();
            _runTimer.Stop();
        }

        public RunController RunController { get; }

        public IList<UserControl> SensorWidgets { get; }

        public IList<Plot> PlotWidgets { get; }

        public string TimerText
        {
            get
            {
                return RunController.CurrentRun == null
                    ? "0:00:00.0"
                    : (DateTime.UtcNow - RunController.CurrentRun.StartTime).ToString("g").Substring(0, 9);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
