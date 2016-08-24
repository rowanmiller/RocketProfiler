// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace RocketProfiler.Controller.DataAccess
{
    public class RocketProfilerContext : DbContext
    {
        private readonly string _datatbaseName;

        public RocketProfilerContext(string datatbaseName)
        {
            _datatbaseName = datatbaseName;
        }

        public DbSet<Run> Runs { get; set; }
        public DbSet<RunSensor> Sensors { get; set; }
        public DbSet<SensorValue> SensorValues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("DataSource=" + _datatbaseName);
    }
}
