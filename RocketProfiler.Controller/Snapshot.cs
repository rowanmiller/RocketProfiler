// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace RocketProfiler.Controller
{
    public class Snapshot
    {
        public int Id { get; set; }
        public int RunId { get; set; }
        public Run Run { get; set; }
        public DateTime Timestamp { get; set; }
        public ICollection<SensorValue> SensorValues { get; } = new List<SensorValue>();
    }
}
