// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RocketProfiler.Controller
{
    public abstract class RunRepository
    {
        protected abstract RocketProfilerContext CreateContext();

        public IList<Run> LoadRuns()
        {
            using (var context = CreateContext())
            {
                return context.Runs.OrderBy(r => r.StartTime).ToList();
            }
        }

        public void PopulateRun(Run run)
        {
            using (var context = CreateContext())
            {
                context.Attach(run);

                context.Runs
                    .Where(r => r.Id == run.Id)
                    .Include(e => e.Snapshots)
                    .ThenInclude(e => e.SensorValues)
                    .ThenInclude(s => s.SensorInfo)
                    .Load();
            }
        }

        public void ExportToSqlServer(Run run, string connectionString)
        {
            if (!run.Snapshots.Any())
            {
                PopulateRun(run);
            }

            var clonedSensors = new Dictionary<int, SensorInfo>();
            var clonedRun = run.Clone(clonedSensors);

            using (var context = new RocketProfilerSqlServerContext(connectionString))
            {
                context.Database.Migrate();

                foreach (var clonedSensor in clonedSensors.Values)
                {
                    var existingSensor = context.Sensors.FirstOrDefault(e => e.Name == clonedSensor.Name);
                    if (existingSensor != null)
                    {
                        clonedSensor.Id = existingSensor.Id;
                        context.Entry(existingSensor).State = EntityState.Detached;
                        context.Entry(clonedSensor).State = EntityState.Unchanged;
                    }
                }

                context.Add(clonedRun);

                context.SaveChanges();
            }
        }
    }
}
