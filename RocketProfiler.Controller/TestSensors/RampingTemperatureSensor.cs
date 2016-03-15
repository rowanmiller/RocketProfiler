// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using RocketProfiler.Controller;

namespace RocketProfiler.Controller.TestSensors
{
    public class RampingTemperatureSensor : TestTemperatureSensor
    {
        public RampingTemperatureSensor(string name, int? sleep = null)
            : base(name, CreateValues(), sleep)
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

        public static RampingTemperatureSensor Create(ICollection<SensorValue> readValues, string name, int? sleep = null)
        {
            var sensor = new RampingTemperatureSensor(name, sleep);

            sensor.LastRead.PropertyChanged += (s, e) => { readValues.Add(((CurrentSensorValue)s).Value); };
            return sensor;
        }
    }
}
