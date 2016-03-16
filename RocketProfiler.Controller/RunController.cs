// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace RocketProfiler.Controller
{
    public class RunController : IDisposable
    {
        private readonly IList<Sensor> _sensors;
        private readonly Runner _runner;
        private readonly IList<Run> _collectedRuns = new List<Run>();
        private bool _initialized;

        public RunController(IList<Sensor> sensors, int pollingInterval)
        {
            _sensors = sensors;
            _runner = new Runner(_sensors, pollingInterval);
        }

        private List<Type> GetSensorTypes() 
            => _sensors.Select(e => e.GetType()).Distinct().ToList();

        public virtual string DatabaseName { get; set; }
            = ("RocketProfiler_" + DateTime.Now + ".rocket").Replace('/', '_').Replace(' ', '_').Replace(':', '_');

        public virtual Run CurrentRun { get; private set; }

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

            using (var context = new RocketProfilerContext(DatabaseName, GetSensorTypes()))
            {
                context.AttachRange(_sensors);
                context.AddRange(_collectedRuns);
                context.SaveChanges();
                _collectedRuns.Clear();
            }
        }

        private void InitializeDatabase()
        {
            if (!_initialized)
            {
                using (var context = new RocketProfilerContext(DatabaseName, GetSensorTypes()))
                {
                    context.Database.EnsureCreated();

                    context.AddRange(_sensors);

                    context.SaveChanges();
                }

                _initialized = true;
            }
        }

        public void Dispose() => _runner.Dispose();
    }
}
