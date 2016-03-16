// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RocketProfiler.Controller
{
    public class RocketProfilerContext : DbContext
    {
        private readonly string _datatbaseName;
        private readonly IList<Type> _sensorTypes;

        public RocketProfilerContext(string datatbaseName, IList<Type> sensorTypes)
        {
            _datatbaseName = datatbaseName;
            _sensorTypes = sensorTypes;
        }

        public DbSet<Run> Runs { get; set; }
        public DbSet<Sensor> Sensors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("DataSource=" + _datatbaseName);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var sensorType in _sensorTypes)
            {
                modelBuilder.Entity(sensorType);
            }
        }
    }
}
