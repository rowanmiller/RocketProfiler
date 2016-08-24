// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RocketProfiler.Controller;
using RocketProfiler.Controller.Hardware;
using RocketProfiler.Controller.Hardware.TestSensors;
using RocketProfiler.UI.ViewModels;
using RocketProfiler.UI.Views;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace RocketProfiler.UI
{
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            new MainView(GetRealRunViewModel()).Show();
        }


        public RunViewModel GetRealRunViewModel()
        {
            var numato = new NumatoGpioModule("COM4");
            Exit += (_, __) => numato.Dispose();

            var oxidizerValve = new GpioRelay("Oxidizer Valve", numato, pin: 8, lowIsOn: true);
            var igniter = new GpioRelay("Ignition", numato, pin: 9, lowIsOn: true);

            var controlSteps = new List<ControlStep>
            {
                new ControlStep("Initial Pause (5 sec)", () => Thread.Sleep(5000)),
                new ControlStep("Oxidizer On", () => oxidizerValve.TurnOn()),
                new ControlStep("Oxidizer Build Up (1 sec)", () => Thread.Sleep(1000)),
                new ControlStep("Igniter On ", () => igniter.TurnOn()),
                new ControlStep("Ignition (2 sec)", () => Thread.Sleep(1000)),
                new ControlStep("Igniter Off ", () => igniter.TurnOff()),
                new ControlStep("Main Burn (10 sec)", () => Thread.Sleep(10000)),
                new ControlStep("Oxidizer Off", () => oxidizerValve.TurnOff()),
            };

            var abortSteps = new List<ControlStep>
            {
                new ControlStep("Oxidizer Off", () => oxidizerValve.TurnOff()),
                new ControlStep("Igniter Off ", () => igniter.TurnOff()),
            };

            var thread = new Thread(new ThreadStart(() => numato.MonitorAndProcessQueue(CancellationToken.None)));
            thread.Start();
            Exit += (_, __) => thread.Abort();

            var temp1 = new AD8495TemperatureSensor("Oxidizer Inlet Temp", 30, numato, 0);
            var temp2 = new MAX6675TemperatureSensor("Lower Engine Temp", 25, numato, 1, 2, 3);

            var sensors = new List<ActiveSensor>
            {
                new ActiveSensor
                {
                    Sensor = temp1,
                    StatusView = new TemperatureSensorWidget(new TemperatureSensorWidgetViewModel(temp1)),
                    PlotView = new TemperaturePlotView(new TemperaturePlotViewModel(temp1))
                },
                new ActiveSensor
                {
                    Sensor = temp2,
                    StatusView = new TemperatureSensorWidget(new TemperatureSensorWidgetViewModel(temp2)),
                    PlotView = new TemperaturePlotView(new TemperaturePlotViewModel(temp2))
                }
            };

            return new RunViewModel(controlSteps, abortSteps, sensors);
        }

        public RunViewModel GetTestRunViewModel()
        {
            var temp1 = new TestTemperatureSensor("Oxidizer Inlet Temp", new List<int> { 70, 75, 80, 85, 75 });
            var temp2 = new RampingTemperatureSensor("Lower Engine Temp");

            var sensors = new List<ActiveSensor>
            {
                new ActiveSensor
                {
                    Sensor = temp1,
                    StatusView = new TemperatureSensorWidget(new TemperatureSensorWidgetViewModel(temp1)),
                    PlotView = new TemperaturePlotView(new TemperaturePlotViewModel(temp1))
                },
                new ActiveSensor
                {
                    Sensor = temp2,
                    StatusView = new TemperatureSensorWidget(new TemperatureSensorWidgetViewModel(temp2)),
                    PlotView = new TemperaturePlotView(new TemperaturePlotViewModel(temp2))
                }
            };

            return new RunViewModel(new List<ControlStep>(), new List<ControlStep>(), sensors);
        }
    }
}
