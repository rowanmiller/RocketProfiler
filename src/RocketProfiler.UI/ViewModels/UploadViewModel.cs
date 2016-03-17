// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RocketProfiler.Controller;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace RocketProfiler.UI.ViewModels
{
    public class UploadViewModel : INotifyPropertyChanged
    {
        private int _progress;
        private string _progressMessage;
        private string _filename;

        public UploadViewModel(string filename)
        {
            _filename = filename;
            Message = "Uploading " + Path.GetFileName(filename);
        }

        public void DoUpload()
        {
            var rep = new SqliteRunRepository(_filename);

            ProgressMessage = "Reading data from local session";
            var runs = rep.LoadRuns();
            Progress = 10;

            for (int i = 0; i < runs.Count; i++)
            {
                ProgressMessage = $"Uploading {i + 1}/{runs.Count} runs";
                rep.ExportToSqlServer(runs[i], App.GetRemoteConnectionString());
                Progress = (i + 1) * 90 / runs.Count + 10;
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        public string ProgressMessage
        {
            get { return _progressMessage; }
            set
            {
                _progressMessage = value;
                OnPropertyChanged();
            }
        }

        public string Message { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
