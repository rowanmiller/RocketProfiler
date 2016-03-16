// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Windows.Controls;
using OxyPlot.Wpf;
using RocketProfiler.Controller;
using RocketProfiler.UI.Views;

namespace RocketProfiler.UI.ViewModels
{
    public class MainWindowViewModel
    {
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
        }

        public RunController RunController { get; }

        public IList<UserControl> SensorWidgets { get; }

        public IList<Plot> PlotWidgets { get; }
    }
}
