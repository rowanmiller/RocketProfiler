// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Win32;
using RocketProfiler.Controller;
using RocketProfiler.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace RocketProfiler.UI.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private IList<Sensor> _sensors;
        private Lazy<RunView> _runView;

        public MainView()
        {
            InitializeComponent();

            _sensors = App.GetTestSensors();

            _runView = new Lazy<RunView>(() =>
            {
                var view = new RunView(
                    new RunViewModel(
                        _sensors,
                        new RunController(_sensors, 300)));

                return view;
            });
        }

        private void CurrentRun_Click(object sender, RoutedEventArgs e)
        {
            DocumentFrame.Navigate(_runView.Value);
        }

        private void OpenSession_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".rocket",
                Filter = "RocketProfiler Sessions (.rocket)|*.rocket",
            };

            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                DocumentFrame.Navigate(
                    new SessionView(
                        new SessionViewModel(
                            new SqliteRunRepository(dlg.FileName),
                            Path.GetFileName(dlg.FileName))));
            }
        }

        private void OpenRemoteSession_Click(object sender, RoutedEventArgs e)
        {
            DocumentFrame.Navigate(
                    new SessionView(
                        new SessionViewModel(
                            new SqlServerRunRepository(App.GetRemoteConnectionString()),
                            "Remote Runs")));
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".rocket",
                Filter = "RocketProfiler Sessions (.rocket)|*.rocket",
            };

            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var viewModel = new UploadViewModel(dlg.FileName);
                var view = new UploadView(viewModel);
                view.Show();

                var thread = new System.Threading.Thread(
                    new System.Threading.ThreadStart(
                        delegate ()
                        {
                            viewModel.DoUpload();

                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                new Action(() =>
                                {
                                    view.Close();
                                }));
                        }
                    ));
                thread.Start();

            }
        }
    }
}
