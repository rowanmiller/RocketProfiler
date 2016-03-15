// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
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
            SensorWidgets = new List<UserControl>();
            PlotWidgets = new List<Plot>();

            foreach (var sensor in sensors)
            {
                SensorWidgets.Add(
                    new TemperatureSensorWidget(
                        new TemperatureSensorWidgetViewModel(sensor)));

                var plot = new Plot();
                plot.Title = sensor.Name;
                plot.Series.Add(
                    new LineSeries
                    {
                        ItemsSource = new SensorPlotWidgetViewModel(runController).DataPoints
                    });

                PlotWidgets.Add(plot);
            }
        }

        public IList<UserControl> SensorWidgets { get; }

        public IList<Plot> PlotWidgets { get; }
    }
}
