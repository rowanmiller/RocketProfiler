// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RocketProfiler.Controller;
using Xunit;
using RocketProfiler.Controller.TestSensors;

namespace RocketProfiler.Test
{
    public class ControllerTest
    {
        [Fact]
        public void RunController_forces_sensor_reads_and_sets_current_value()
            => RunControllerTest(10, null);

        [Fact]
        public void RunController_handles_slow_sensors()
            => RunControllerTest(1, 10);

        private static void RunControllerTest(int pollingInterval, int? sensorSleep)
        {
            var readValues = new List<SensorValue>();
            var sensor = RampingTemperatureSensor.Create(readValues, "Sensor1", sensorSleep);

            using (new RunController(new Sensor[] { sensor }, pollingInterval))
            {
                Thread.Sleep(100);

                Assert.InRange(readValues.Count, 5, 15);

                Assert.Equal(new List<double> { 0, 10, 20, 30, 40 },
                    readValues.Take(5).Select(v => v.Value).ToList());
            }
        }

        [Fact]
        public void Controller_can_record_run()
        {
            var readValues1 = new List<SensorValue>();
            var sensor1 = RampingTemperatureSensor.Create(readValues1, "Sensor1");
            var readValues2 = new List<SensorValue>();
            var sensor2 = RampingTemperatureSensor.Create(readValues2, "Sensor1");

            Run run;
            using (var controller = new RunController(new Sensor[] { sensor1, sensor2 }, 10))
            {
                Assert.Null(controller.LastRun);

                Thread.Sleep(30);

                controller.StartRecoding("TestRun", "A test run.");
                Thread.Sleep(100);
                controller.StopRecording();

                Thread.Sleep(30);

                run = controller.LastRun;
            }

            Assert.NotNull(run);

            Assert.Equal("TestRun", run.Name);
            Assert.Equal("A test run.", run.Description);

            Assert.InRange(run.Snapshots.Count, 5, 15);

            var lastTimestamp = new DateTime();
            var valuesPos = -1;

            foreach (var snapshot in run.Snapshots)
            {
                Assert.Equal(2, snapshot.SensorValues.Count);
                Assert.True(snapshot.Timestamp >= lastTimestamp);
                lastTimestamp = snapshot.Timestamp;

                var sensorValue1 = snapshot.SensorValues.First();
                var sensorValue2 = snapshot.SensorValues.Skip(1).First();

                if (valuesPos == -1)
                {
                    while (readValues1[++valuesPos] != sensorValue1)
                    {
                    }
                    Assert.True(valuesPos > 0);
                }
                else
                {
                    Assert.Same(readValues1[++valuesPos], sensorValue1);
                }
                Assert.Same(readValues2[valuesPos], sensorValue2);
            }

            Assert.True(valuesPos < readValues1.Count);
        }
    }
}
