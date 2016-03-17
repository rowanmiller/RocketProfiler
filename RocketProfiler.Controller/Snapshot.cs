// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace RocketProfiler.Controller
{
    public class Snapshot
    {
        public int Id { get; set; }
        public int RunId { get; set; }
        public Run Run { get; set; }
        public DateTime Timestamp { get; set; }
        public IList<SensorValue> SensorValues { get; private set; } = new List<SensorValue>();

        public Snapshot Clone(Run clonedRun, IDictionary<int, SensorInfo> clonedSensors)
        {
            var clone = new Snapshot
            {
                Run = clonedRun,
                Timestamp = Timestamp
            };

            clone.SensorValues = SensorValues.Select(s => s.Clone(clone, clonedSensors)).ToList();

            return clone;
        }
    }
}
