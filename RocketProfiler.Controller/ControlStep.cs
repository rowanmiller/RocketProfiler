using System;

namespace RocketProfiler.Controller
{
    public class ControlStep
    {
        private Action _action;

        public ControlStep(string name, Action action)
        {
            Name = name;
            _action = action;
        }

        public string Name { get; }

        public void Execute()
        {
            RaiseStartedEvent();
            _action();
            RaiseCompletedEvent();
        }

        public delegate void ControlStepStatusEventHandler(object sender);

        public event ControlStepStatusEventHandler Started;
        public event ControlStepStatusEventHandler Completed;

        protected virtual void RaiseStartedEvent()
        {
            Started?.Invoke(this);
        }

        protected virtual void RaiseCompletedEvent()
        {
            Completed?.Invoke(this);
        }
    }
}
