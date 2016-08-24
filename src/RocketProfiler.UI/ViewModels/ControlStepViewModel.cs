using RocketProfiler.Controller;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace RocketProfiler.UI.ViewModels
{
    public class ControlStepViewModel : INotifyPropertyChanged
    {
        public ControlStepViewModel(ControlStep step)
        {
            Step = step;

            Step.Started += (_) =>
            {
                IsStarted = true;

                OnPropertyChanged(nameof(IsStarted));
                OnPropertyChanged(nameof(Status));
            };

            Step.Completed += (_) =>
            {
                Completed = DateTime.UtcNow;
                IsCompleted = true;

                OnPropertyChanged(nameof(Completed));
                OnPropertyChanged(nameof(IsCompleted));
                OnPropertyChanged(nameof(Status));
            };
        }

        public ControlStep Step { get; }
        public DateTime Completed { get; set; }
        public bool IsStarted { get; private set; }
        public bool IsCompleted { get; private set; }

        public string Status
        {
            get
            {
                return IsCompleted
                    ? $"Complete: {Completed.ToLocalTime().ToLongTimeString()}"
                    : IsStarted
                        ? "In Progress"
                        : "Not Started";
            }

        }

        public void Reset()
        {
            IsStarted = false;
            IsCompleted = false;

            OnPropertyChanged(nameof(Completed));
            OnPropertyChanged(nameof(IsCompleted));
            OnPropertyChanged(nameof(Status));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
