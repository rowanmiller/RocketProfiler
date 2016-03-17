// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using RocketProfiler.Controller;
using RocketProfiler.UI.Views;
using TimeSpanAxis = OxyPlot.Wpf.TimeSpanAxis;

namespace RocketProfiler.UI.ViewModels
{
    public class RunViewModel : INotifyPropertyChanged
    {
        private readonly Timer _runTimer;

        public RunViewModel(IEnumerable<Sensor> sensors, RunController runController)
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

                plotViewModel.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == "DataPoints")
                        {
                            Application.Current.Dispatcher.InvokeAsync(() =>
                                plot.InvalidatePlot());
                        }
                    };

                PlotWidgets.Add(plot);
            }

            _runTimer = new Timer(100);
            _runTimer.Elapsed += (_, __) => { OnPropertyChanged(nameof(TimerText)); };
        }

        public void StartRun(string runName, string runDescription)
        {
            RunController.StartRecoding(runName, runDescription);
            _runTimer.Start();
        }

        public void StopRun()
        {
            RunController.StopRecording();
            RunController.PersistRuns();
            _runTimer.Stop();
        }

        public RunController RunController { get; }

        public IList<UserControl> SensorWidgets { get; }

        public IList<Plot> PlotWidgets { get; }

        public string TimerText
        {
            get
            {
                lock (RunController.Lock)
                {
                    return RunController.CurrentRun == null
                        ? "0:00:00.0"
                        : (DateTime.UtcNow - RunController.CurrentRun.StartTime).ToString("g").Substring(0, 9);
                }
            }
        }

        public string SessionFilePath
        {
            get { return RunController.DatabaseName; }
            set
            {
                RunController.DatabaseName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
