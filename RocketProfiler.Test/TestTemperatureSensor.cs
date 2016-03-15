// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using RocketProfiler.Controller;

namespace RocketProfiler.Test
{
    public class TestTemperatureSensor : Sensor
    {
        private readonly IList<int> _values;
        private int _index = -1;

        public TestTemperatureSensor(string name, IList<int> values)
        {
            _values = values;
            Name = name;
        }

        public override SensorValue DoRead()
            => new TemperatureSensorValue
            {
                Sensor = this,
                Temperature = _values[_index = (_index + 1) % _values.Count],
                Timestamp = DateTime.UtcNow
            };
    }
}
