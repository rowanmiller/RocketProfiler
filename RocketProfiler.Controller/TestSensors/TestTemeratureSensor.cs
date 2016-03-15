// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace RocketProfiler.Controller.TestSensors
{
    public class TestTemperatureSensor : Sensor
    {
        private readonly IList<int> _values;
        private readonly int? _sleep;
        private int _index = -1;

        public TestTemperatureSensor(string name, IList<int> values, int? sleep = null)
        {
            _values = values;
            _sleep = sleep;
            Name = name;
        }

        public override string Units { get; } = "Test Units";

        public override double MaxValue => _values.Max() * 2;

        public override SensorValue DoRead()
        {
            if (_sleep != null)
            {
                Thread.Sleep(_sleep.Value);
            }

            return new SensorValue
            {
                Sensor = this,
                Value = _values[_index = (_index + 1) % _values.Count],
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
