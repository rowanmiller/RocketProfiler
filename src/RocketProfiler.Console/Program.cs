using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using RocketProfiler.Controller;
using RocketProfiler.Controller.Hardware;

namespace RocketProfiler.ConsoleHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var gpio = new NumatoGpioModule("COM4"))
            {
                var sensor1 = new AD8495TemperatureSensor("Analog Sensor", 100, gpio, 0);
                sensor1.SensorReadEvent += (sender, e) =>
                {
                    Console.WriteLine($"AD8495:  {e.Value}");
                };

                var sensor2 = new MAX6675TemperatureSensor("Digital Sensor", 400, gpio, 1, 2, 3);
                sensor2.SensorReadEvent += (sender, e) =>
                {
                    Console.WriteLine($"MAX6675: {e.Value}");
                };

                var relay = new GpioRelay("Oxidizer", gpio, 8, true);
                relay.TurnOff();
                relay.TurnOn();

                gpio.MonitorAndProcessQueue(CancellationToken.None);

                
            }
        }
    }
}
