// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace RocketProfiler.Controller
{
    public class SensorInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Units { get; set; }
        public double MaxValue { get; set; }

        // Because Sqlite Migrations can't drop the column and it is non-nullable
        [Required]
        public string Discriminator { get; set; } = "";

        public SensorInfo Clone()
            => new SensorInfo
            {
                Name = Name,
                Units = Units,
                MaxValue = MaxValue
            };
    }
}
