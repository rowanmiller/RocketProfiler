// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace RocketProfiler.Controller
{
    public class Runner : IDisposable
    {
        private readonly int _pollingInterval;
        private readonly object _lock = new object();
        private readonly IList<Sensor> _sensors;
        private readonly Timer _timer;
        private readonly DateTime _startedAt;
        private int _count;
        private bool _stopped;
        private bool _sampling;

        private Run _run;

        public Runner(IList<Sensor> sensors, int pollingInterval)
        {
            _sensors = sensors;
            _startedAt = DateTime.UtcNow;
            _timer = new Timer(_ => SampleSensors(), null, 0, Timeout.Infinite);
            _pollingInterval = pollingInterval;
        }

        public void RecordRun(Run run)
        {
            lock (_lock)
            {
                _run = run;
            }
        }

        public void EndRun()
        {
            lock (_lock)
            {
                _run = null;
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

                var fireAtNext = Math.Max(
                    0,
                    (int)(_startedAt + TimeSpan.FromMilliseconds(++_count * _pollingInterval))
                        .Subtract(DateTime.UtcNow)
                        .TotalMilliseconds);

                _timer.Change(fireAtNext, Timeout.Infinite);

                if (_sampling)
                {
                    return;
                }

                _sampling = true;
            }

            var snapshot = new Snapshot { Run = _run, Timestamp = DateTime.UtcNow };

            foreach (var sensor in _sensors)
            {
                snapshot.SensorValues.Add(sensor.ReadValue());
            }

            lock (_lock)
            {
                if (!_stopped)
                {
                    _run?.Snapshots.Add(snapshot);
                }
                _sampling = false;
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _stopped = true;
                _timer.Dispose();
            }
        }
    }
}
