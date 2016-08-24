// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RocketProfiler.Controller;
using RocketProfiler.UI.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Controls;

namespace RocketProfiler.UI.ViewModels
{
    public class RunViewModel : INotifyPropertyChanged
    {
        private readonly RunController _runController;
        private readonly IEnumerable<ActiveSensor> _activeSensors;

        public RunViewModel(IEnumerable<ControlStep> controlSteps, IEnumerable<ControlStep> abortSteps, IEnumerable<ActiveSensor> activeSensors)
        {
            _runController = new RunController(controlSteps, abortSteps, activeSensors.Select(s => s.Sensor));
            _activeSensors = activeSensors;

            ControlSteps = controlSteps.Select(s => new ControlStepViewModel(s)).ToList();

            var timer = new Timer(100);
            timer.Elapsed += (_, __) =>
            {
                OnPropertyChanged(nameof(TimerText));
            };
            timer.Start();
        }

        public void StartRun(string runName, string runDescription)
        {
            foreach (var item in PlotWidgets)
            {
                item.ViewModel.Restart();
            }

            foreach (var step in ControlSteps)
            {
                step.Reset();
            }

            _runController.Start(runName, runDescription);
        }

        public void StopRun()
        {
            _runController.Stop();

            foreach (var item in PlotWidgets)
            {
                item.ViewModel.Restart();
            }
        }

        public IEnumerable<UserControl> SensorWidgets => _activeSensors.Select(s => s.StatusView);

        public IEnumerable<PlotView> PlotWidgets => _activeSensors.Select(s => s.PlotView);

        public IEnumerable<ControlStepViewModel> ControlSteps { get; } 

        public string TimerText
        {
            get
            {
                return _runController.CurrentRun == null
                    ? "0:00:00.0"
                    : (DateTime.UtcNow - _runController.CurrentRun.StartTime).ToString("g").Substring(0, 9);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
