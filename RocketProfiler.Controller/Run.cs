// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace RocketProfiler.Controller
{
    public class Run
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public IList<Snapshot> Snapshots { get; private set; } = new List<Snapshot>();

        public Run Clone(IDictionary<int, SensorInfo> clonedSensors)
        {
            var clone = new Run
            {
                Name = Name,
                Description = Description,
                StartTime = StartTime,
                EndTime = EndTime,
            };

            clone.Snapshots = Snapshots.Select(s => s.Clone(clone, clonedSensors)).ToList();

            return clone;
        }
    }
}
