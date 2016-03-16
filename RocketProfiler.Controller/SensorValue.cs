// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace RocketProfiler.Controller
{
    public class SensorValue
    {
        public int Id { get; set; }

        public int SnapshotId { get; set; }
        public Snapshot Snapshot { get; set; }

        public Sensor Sensor { get; set; }

        public DateTime Timestamp { get; set; }

        public double? Value { get; set; }
    }
}
