// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RocketProfiler.Controller
{
    public class RunRepository
    {
        private readonly string _datatbaseName;

        public RunRepository(string filename)
        {
            _datatbaseName = filename;
        }

        public IList<Run> LoadRuns()
        {
            using (var context = new RocketProfilerContext(_datatbaseName))
            {
                return context.Runs.OrderBy(r => r.StartTime).ToList();
            }
        }

        public void PopulateRun(Run run)
        {
            using (var context = new RocketProfilerContext(_datatbaseName))
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
    }
}
