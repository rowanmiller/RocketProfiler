// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RocketProfiler.Controller.Hardware.TestSensors
{
    public class TestTemperatureSensor : Sensor
    {
        private readonly IList<int> _values;
        private readonly int? _sleep;
        private int _index = -1;

        public TestTemperatureSensor(string title, IList<int> values, int period = 100)
            : base(title: title, units: "Test Units", minValue: 0, maxValue: values.Max() * 2, threshold: values.Average())
        {
            _values = values;
            _sleep = period;

            var timer = new Timer(
                callback: _ => RaiseSampleEvent(new SensorReadEventArgs(_values[_index = (_index + 1) % _values.Count], DateTime.UtcNow)),
                state: null,
                dueTime: period,
                period: period);
        }
    }
}
