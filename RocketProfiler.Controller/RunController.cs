// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RocketProfiler.Controller.DataAccess;
using RocketProfiler.Controller.Hardware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace RocketProfiler.Controller
{
    public class RunController
    {
        private bool _recording;
        private Run _currentRun;
        private Thread _controlThread;
        public static string DEFAULT_RESULTS_DIRECTORY = @"C:\RocketProfiler\Runs\";

        public RunController(IEnumerable<ControlStep> controlSteps, IEnumerable<ControlStep> abortSteps, IEnumerable<Sensor> sensors)
        {
            ControlSteps = controlSteps;
            AbortSteps = abortSteps;
            Sensors = sensors;
        }

        public IEnumerable<ControlStep> ControlSteps { get; private set; }
        public IEnumerable<ControlStep> AbortSteps { get; private set; }
        public IEnumerable<Sensor> Sensors { get; private set; }

        public Run CurrentRun => _currentRun;

        public virtual void Start(string runName, string runDescription)
        {
            if (_currentRun != null)
            {
                throw new InvalidOperationException("Run already in progress.");
            }

            _currentRun = new Run
            {
                Name = runName,
                Description = runDescription,
                StartTime = DateTime.UtcNow
            };

            _controlThread = new Thread(new ThreadStart(() =>
            {
                foreach (var step in ControlSteps)
                {
                    step.Execute();
                }
            }));

            foreach (var sensor in Sensors)
            {
                var sensorInfo = new RunSensor(sensor);
                _currentRun.Sensors.Add(sensorInfo);

                sensor.SensorReadEvent += (_, e) =>
                {
                    if (_recording)
                    {
                        sensorInfo.Values.Add(new SensorValue(e) { SensorInfo = sensorInfo });
                    }
                };
            }

            _recording = true;

            _controlThread.Start();
        }

        public virtual void Stop()
        {
            if(_controlThread.IsAlive)
            {
                _controlThread.Abort();
                foreach (var step in AbortSteps)
                {
                    step.Execute();
                }
            }

            _recording = false;
            _currentRun.EndTime = DateTime.UtcNow;

            // TODO Unwire event notifications

            var file = new FileInfo(Path.Combine(DEFAULT_RESULTS_DIRECTORY, $@"{DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss")} {_currentRun.Name}.rocket"));
            using (var context = new RocketProfilerContext(file.FullName))
            {
                if(!Directory.Exists(file.Directory.FullName))
                {
                    Directory.CreateDirectory(file.Directory.FullName);
                }

                context.Database.EnsureCreated();
                context.Runs.Add(_currentRun);
                context.SaveChanges();
            }
            
            _currentRun = null;
            _controlThread = null;
        }
    }
}
