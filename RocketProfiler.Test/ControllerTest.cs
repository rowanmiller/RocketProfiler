// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
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

            var controller = new RunController(new Sensor[] { sensor });

            controller.StartRecoding("TestRun", "A test run.");
        }
    }

    public class TestTemperatureSensor : Sensor
    {
        private readonly IList<int> _values;
        private int _index = -1;

        public TestTemperatureSensor(string name, IList<int> values)
        {
            _values = values;
            Name = name;
        }

        public override SensorValue ReadValue() 
            => new TemperatureSensorValue
        {
            Sensor = this,
            Temperature = _values[_index = (_index + 1) % _values.Count],
            Timestamp = DateTime.UtcNow
        };

        public override SensorValue DoRead()
            => new TemperatureSensorValue
            {
                Sensor = this,
                Temperature = _values[_index = (_index + 1) % _values.Count],
                Timestamp = DateTime.UtcNow
            };
    }

    public class RampingTemperatureSensor : TestTemperatureSensor
    {
        public RampingTemperatureSensor(string name)
            : base(name, CreateValues())
        {
        }

        private static IList<int> CreateValues()
        {
            var values = new List<int>();

            for (var i = 0; i < 1000; i += 10)
            {
                values.Add(i);
            }
            for (var i = 1000; i >= 0; i -= 10)
            {
                values.Add(i);
            }

            return values;
        }
    }
}
