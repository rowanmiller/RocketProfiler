// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RocketProfiler.Controller
{
    public class RunController : IRunController
    {
        private readonly IList<Sensor> _sensors;
        private Runner _runner;

        public RunController(IList<Sensor> sensors)
        {
            _sensors = sensors;
        }

        public virtual Run LastRun { get; private set; }

        public virtual void StartRecoding(string runName, string runDescription)
        {
            Debug.Assert(_runner == null);

            LastRun = new Run
            {
                Name = runName,
                Description = runDescription,
                StartTime = DateTime.UtcNow
            };

            _runner = new Runner(LastRun, _sensors);

            _runner.Start();
        }

        public virtual void StopRecording()
        {
            _runner?.Stop();
            _runner = null;
        }

        public virtual void PersistLastRun()
        {
            Debug.Assert(LastRun != null);

            //TODO
        }
    }
}
