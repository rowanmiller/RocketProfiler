// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using RocketProfiler.Controller;
using TimeSpanAxis = OxyPlot.Wpf.TimeSpanAxis;

namespace RocketProfiler.UI.ViewModels
{
    public class SessionViewModel : INotifyPropertyChanged
    {
        private readonly List<HistoryPlotViewModel> _plotViewModels = new List<HistoryPlotViewModel>();

        public SessionViewModel(IList<Sensor> sensors, string sessionFile)
        {
            Title = Path.GetFileName(sessionFile);

            RunRepository = new SqliteRunRepository(sessionFile);

            Runs = RunRepository.LoadRuns();

            PlotWidgets = new List<Plot>();

            foreach (var sensor in sensors)
            {
                var plotViewModel = new HistoryPlotViewModel(sensor);

                var plot = new Plot {Title = sensor.Info.Name};
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
        }

        public RunRepository RunRepository { get; }

        public string Title { get; private set; }

        public IList<Run> Runs { get; private set; }

        public Run CurrentRun
        {
            set
            {
                RunRepository.PopulateRun(value);

                foreach (var viewModel in _plotViewModels)
                {
                    viewModel.UpdateRun(value);
                }
            }
        }

        public IList<Plot> PlotWidgets { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
