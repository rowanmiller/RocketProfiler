// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using RocketProfiler.Controller;
using RocketProfiler.Controller.TestSensors;
using Xunit;

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

                Assert.Equal(new List<double?> { 0, 10, 20, 30, 40 },
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
                Assert.Null(controller.CurrentRun);

                Thread.Sleep(30);

                controller.StartRecoding("TestRun", "A test run.");
                Thread.Sleep(100);
                controller.StopRecording();

                Thread.Sleep(30);

                run = controller.CurrentRun;
            }

            Assert.NotNull(run);

            Assert.Equal("TestRun", run.Name);
            Assert.Equal("A test run.", run.Description);
            Assert.True(run.StartTime > new DateTime());
            Assert.True(run.StartTime < run.EndTime);

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

        [Fact]
        public void Controller_can_persist_runs()
        {
            var sensors = new Sensor[]
            {
                new RampingTemperatureSensor("Sensor1"),
                new RampingTemperatureSensor("Sensor1")
            };

            string databaseName;
            Run run1;
            Run run2;

            using (var controller = new RunController(sensors, 70))
            {
                Assert.Null(controller.CurrentRun);

                Thread.Sleep(30);

                controller.StartRecoding("TestRun1", "A test run.");
                run1 = controller.CurrentRun;
                Thread.Sleep(500);
                controller.StopRecording();

                Thread.Sleep(30);

                controller.StartRecoding("TestRun2", "A test run again.");
                run2 = controller.CurrentRun;
                Thread.Sleep(500);
                controller.StopRecording();

                Thread.Sleep(30);

                controller.PersistRuns();

                databaseName = controller.DatabaseName;
            }

            using (var context = new RocketProfilerContext(databaseName, new[] { typeof(RampingTemperatureSensor) }))
            {
                var runs = context.Runs.Include(e => e.Snapshots).ThenInclude(e => e.SensorValues).ToList();
                Assert.Equal(2, runs.Count);

                AssertRunsEqual(run1, runs[0]);
                AssertRunsEqual(run2, runs[1]);
            }
        }

        private static void AssertRunsEqual(Run expectedRun, Run actualRun)
        {
            Assert.Equal(expectedRun.Name, actualRun.Name);
            Assert.Equal(expectedRun.Description, actualRun.Description);
            Assert.Equal(expectedRun.StartTime, actualRun.StartTime);
            Assert.Equal(expectedRun.EndTime, actualRun.EndTime);

            var snapshots1 = actualRun.Snapshots.OrderBy(e => e.Timestamp).ToList();
            for (var i = 0; i < expectedRun.Snapshots.Count; i++)
            {
                Assert.Equal(expectedRun.Snapshots[i].Timestamp, snapshots1[i].Timestamp);
                for (var j = 0; j < 2; j++)
                {
                    var sensorValues1 = snapshots1[i].SensorValues.OrderBy(e => e.Timestamp).ToList();

                    Assert.Equal(expectedRun.Snapshots[i].SensorValues[j].Timestamp, sensorValues1[j].Timestamp);
                    Assert.Equal(expectedRun.Snapshots[i].SensorValues[j].Value, sensorValues1[j].Value);
                }
            }
        }
    }
}
