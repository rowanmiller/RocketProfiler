// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private readonly IList<HistoryPlotViewModel> _plotViewModels;
        private readonly Timer _runTimer;

        public RunViewModel(IEnumerable<Sensor> sensors, RunController runController)
        {
            RunController = runController;

            SensorWidgets = new List<UserControl>();
            PlotWidgets = new List<Plot>();

             _plotViewModels = new List<HistoryPlotViewModel>();

            foreach (var sensor in sensors)
            {
                SensorWidgets.Add(
                    new TemperatureSensorWidget(
                        new TemperatureSensorWidgetViewModel(sensor)));

                var plotViewModel = new HistoryPlotViewModel(sensor.Info.Name);

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

                _plotViewModels.Add(plotViewModel);
                PlotWidgets.Add(plot);
            }

            _runTimer = new Timer(100);
            _runTimer.Elapsed += (_, __) => { RefreshDisplay(); };
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

        private void RefreshDisplay()
        {
            OnPropertyChanged(nameof(TimerText));

            if (RunController.CurrentRun != null)
            {
                DateTime startTime;
                IEnumerable<IGrouping<string, SensorValue>> dataSeries;

                lock (RunController.Lock)
                {
                    startTime = RunController.CurrentRun.StartTime;

                    dataSeries = RunController
                        .CurrentRun
                        .Snapshots
                        .SelectMany(s => s.SensorValues)
                        .GroupBy(s => s.SensorInfo.Name)
                        .ToList();
                }

                foreach (var sensorData in dataSeries)
                {
                    var viewModel = _plotViewModels.Single(p => p.SensorName == sensorData.Key);

                    viewModel.UpdatePlot(startTime, sensorData);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
