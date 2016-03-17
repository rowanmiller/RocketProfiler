// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RocketProfiler.Controller
{
    public class RunController : IDisposable
    {
        private readonly IList<SensorInfo> _sensorInfos;
        private readonly Runner _runner;
        private readonly IList<Run> _collectedRuns = new List<Run>();
        private bool _initialized;

        public RunController(IList<Sensor> sensors, int pollingInterval)
        {
            _sensorInfos = sensors.Select(s => s.Info).ToList();
            _runner = new Runner(sensors, pollingInterval, Lock);
        }

        public virtual string DatabaseName { get; set; }
            = ("RocketProfiler_" + DateTime.Now + ".rocket").Replace('/', '_').Replace(' ', '_').Replace(':', '_');

        public virtual Run CurrentRun { get; private set; }

        public object Lock { get; } = new object();

        public virtual void StartRecoding(string runName, string runDescription)
            => _runner.RecordRun(CurrentRun = new Run
            {
                Name = runName,
                Description = runDescription,
                StartTime = DateTime.UtcNow
            });

        public virtual void StopRecording()
        {
            CurrentRun.EndTime = DateTime.UtcNow;

            _runner.EndRun();

            _collectedRuns.Add(CurrentRun);
        }

        public virtual void PersistRuns()
        {
            InitializeDatabase();

            using (var context = new RocketProfilerContext(DatabaseName))
            {
                context.AttachRange(_sensorInfos);
                context.AddRange(_collectedRuns);
                context.SaveChanges();
                _collectedRuns.Clear();
            }
        }

        private void InitializeDatabase()
        {
            if (!_initialized)
            {
                using (var context = new RocketProfilerContext(DatabaseName))
                {
                    context.Database.Migrate();

                    context.AddRange(_sensorInfos);

                    context.SaveChanges();
                }

                _initialized = true;
            }
        }

        public void Dispose() => _runner.Dispose();
    }
}
