// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RocketProfiler.Controller
{
    public abstract class RocketProfilerContext : DbContext
    {
        public DbSet<Run> Runs { get; set; }
        public DbSet<SensorInfo> Sensors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder
                .Entity<SensorValue>()
                .HasOne(e => e.SensorInfo)
                .WithMany()
                .HasForeignKey(e => e.SensorId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
