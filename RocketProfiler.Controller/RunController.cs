// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RocketProfiler.Controller
{
    public class RunController : IDisposable
    {
        private readonly IList<Sensor> _sensors;
        private readonly Runner _runner;
        private readonly IList<Run> _runs = new List<Run>();

        public RunController(IList<Sensor> sensors, int pollingInterval)
        {
            _sensors = sensors;
            _runner = new Runner(_sensors, pollingInterval);
        }

        public virtual Run LastRun => _runs.LastOrDefault();

        public virtual void StartRecoding(string runName, string runDescription)
        {
            var run = new Run
            {
                Name = runName,
                Description = runDescription,
                StartTime = DateTime.UtcNow
            };

            _runs.Add(run);
            _runner.RecordRun(run);
        }

        public virtual void StopRecording() => _runner.EndRun();

        public virtual void PersistLastRun()
        {
            Debug.Assert(LastRun != null);

            //TODO
        }

        public void Dispose() => _runner.Dispose();
    }
}
