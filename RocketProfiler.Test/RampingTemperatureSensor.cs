﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace RocketProfiler.Test
{
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
