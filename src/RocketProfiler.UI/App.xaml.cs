// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;
using RocketProfiler.Controller;
using RocketProfiler.Controller.TestSensors;
using RocketProfiler.UI.Views;

namespace RocketProfiler.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
            => new MainView().Show();

        public static List<Sensor> GetRealSensors()
        {
            var numatoPort = new SerialPort();
            numatoPort.PortName = "COM4";
            numatoPort.BaudRate = 9600;
            numatoPort.Open();

            return new List<Sensor>
            {
                new AD8495TemperatureSensor("Oxidizer Inlet Temp", numatoPort, 0),
                new MAX6675TemperatureSensor("Lower Engine Temp", numatoPort, 1, 2, 3)
            };
        }

        public static List<Sensor> GetTestSensors()
            => new List<Sensor>
            {
                new TestTemperatureSensor("Oxidizer Inlet Temp", new List<int> { 70, 75, 80, 85, 75 }),
                new RampingTemperatureSensor("Lower Engine Temp")
            };
    }
}
