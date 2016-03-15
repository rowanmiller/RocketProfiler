// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RocketProfiler.Controller;
using Xunit;

namespace RocketProfiler.Test
{
    public class ControllerTest
    {
        [Fact]
        public void RunController_forces_sensor_reads_and_sets_current_value()
        {
            var sensor = new RampingTemperatureSensor("Sensor1");

            var readValues = new List<SensorValue>();
            sensor.LastRead.PropertyChanged += (s, e) => { readValues.Add(((CurrentSensorValue)s).Value); };

            var controller = new RunController(new Sensor[] { sensor }, 10);

            controller.StartRecoding("TestRun", "A test run.");
            Thread.Sleep(200);
            controller.StopRecording();

            Assert.True(readValues.Count >= 5);

            Assert.Equal(new List<int> { 0, 10, 20, 30, 40 },
                readValues.Take(5).Select(v => ((TemperatureSensorValue)v).Temperature));
        }
    }
}
