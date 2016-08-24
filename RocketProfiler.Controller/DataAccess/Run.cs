// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace RocketProfiler.Controller.DataAccess
{
    public class Run
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ICollection<RunSensor> Sensors { get; set; } = new HashSet<RunSensor>();
    }
}
