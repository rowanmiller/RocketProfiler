// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace RocketProfiler.Controller
{
    public class Runner
    {
        private readonly int _pollingInterval;
        private readonly object _lock = new object();
        private readonly Run _run;
        private readonly IList<Sensor> _sensors;
        private readonly Timer _timer;
        private DateTime _startedAt;
        private int _count;
        private bool _stopped;

        public Runner(Run run, IList<Sensor> sensors, int pollingInterval)
        {
            _run = run;
            _sensors = sensors;
            _timer = new Timer(_ => SampleSensors(), null, Timeout.Infinite, Timeout.Infinite);
            _pollingInterval = pollingInterval;
        }

        public void Start()
        {
            _startedAt = DateTime.UtcNow;
            _timer.Change(0, Timeout.Infinite);
        }

        public void Stop()
        {
            lock (_lock)
            {
                _stopped = true;
                _timer.Dispose();
            }
        }

        public void SampleSensors()
        {
            lock (_lock)
            {
                if (_stopped)
                {
                    return;
                }

                var fireAtNext = (_startedAt + TimeSpan.FromMilliseconds(++_count * _pollingInterval)).Subtract(DateTime.UtcNow);
                _timer.Change((int)fireAtNext.TotalMilliseconds, -1);

                var snapshot = new Snapshot { Run = _run, Timestamp = DateTime.UtcNow };

                foreach (var sensor in _sensors)
                {
                    snapshot.SensorValues.Add(sensor.ReadValue());
                }

                _run.Snapshots.Add(snapshot);
            }
        }
    }
}
