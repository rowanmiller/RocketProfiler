// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Windows;
using RocketProfiler.Controller;
using RocketProfiler.Controller.TestSensors;
using RocketProfiler.UI.ViewModels;
using RocketProfiler.UI.Views;

namespace RocketProfiler.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {

            var sensors = new List<Sensor>
            {
                new TestTemperatureSensor("Fuel Nozzle Temp", new List<int> { 0, 10, 20, 30, 40 }),
                new TestTemperatureSensor("Burner Temp", new List<int> { 500, 503, 600, 789, 450 }),
            };

            var window = new MainWindow(
                new MainWindowViewModel(
                    sensors,
                    new RunController(sensors, 200)));

            window.Show();
        }
    }
}
