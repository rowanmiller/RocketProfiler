// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Win32;
using RocketProfiler.Controller;
using RocketProfiler.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

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
                DocumentFrame.Navigate(new SessionView(new SessionViewModel(_sensors, dlg.FileName)));
            }
        }
    }
}
