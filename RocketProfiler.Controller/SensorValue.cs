// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace RocketProfiler.Controller
{
    public class SensorValue
    {
        public int Id { get; set; }

        public int SnapshotId { get; set; }
        public Snapshot Snapshot { get; set; }

        public int SensorId { get; set; }
        public SensorInfo SensorInfo { get; set; }

        public DateTime Timestamp { get; set; }

        public double? Value { get; set; }

        public SensorValue Clone(Snapshot clonedSnapshot, IDictionary<int, SensorInfo> clonedSensors)
        {
            if (!clonedSensors.ContainsKey(SensorId))
            {
                clonedSensors[SensorId] = SensorInfo.Clone();
            }

            var clone = new SensorValue
            {
                Snapshot = clonedSnapshot,
                SensorInfo = clonedSensors[SensorId],
                Timestamp = Timestamp,
                Value = Value
            };

            return clone;
        }
    }
}
