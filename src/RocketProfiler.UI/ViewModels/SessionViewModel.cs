// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace RocketProfiler.UI.ViewModels
{
    public class SessionViewModel : INotifyPropertyChanged
    {
        public SessionViewModel(string sessionFile)
        {
            Title = Path.GetFileName(sessionFile);
        }

        public string Title { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
