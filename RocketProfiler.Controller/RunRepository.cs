// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RocketProfiler.Controller
{
    public class RunRepository
    {
        private readonly string _datatbaseName;
        private readonly IList<Type> _sensorTypes;

        public RunRepository(string filename, IList<Type> sensorTypes)
        {
            _datatbaseName = filename;
            _sensorTypes = sensorTypes;
        }

        public IList<Run> LoadRuns()
        {
            using (var context = new RocketProfilerContext(_datatbaseName, _sensorTypes))
            {
                return context.Runs.OrderBy(r => r.StartTime).ToList();
            }
        }

        public void PopulateRun(Run run)
        {
            using (var context = new RocketProfilerContext(_datatbaseName, _sensorTypes))
            {
                context.Attach(run);

                context.Runs
                    .Where(r => r.Id == run.Id)
                    .Include(e => e.Snapshots)
                    .ThenInclude(e => e.SensorValues)
                    .Load();
            }
        }
    }
}
