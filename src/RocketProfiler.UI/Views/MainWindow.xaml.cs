// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using RocketProfiler.UI.ViewModels;

namespace RocketProfiler.UI.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = mainWindowViewModel;

            _mainWindowViewModel = mainWindowViewModel;

            for (var index = 0; index < mainWindowViewModel.SensorWidgets.Count; index++)
            {
                SensorGrid.RowDefinitions.Add(new RowDefinition());

                Grid.SetRow(mainWindowViewModel.SensorWidgets[index], index);
                Grid.SetColumn(mainWindowViewModel.SensorWidgets[index], 0);
                SensorGrid.Children.Add(mainWindowViewModel.SensorWidgets[index]);

                Grid.SetRow(mainWindowViewModel.PlotWidgets[index], index);
                Grid.SetColumn(mainWindowViewModel.PlotWidgets[index], 1);
                SensorGrid.Children.Add(mainWindowViewModel.PlotWidgets[index]);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.StartRun(RunName.Text, RunDescription.Text);
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            Inputs.IsEnabled = false;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.StopRun();
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            Inputs.IsEnabled = true;
        }

        private void SelectSessionButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                FileName = "MySession",
                DefaultExt = ".rocket",
                Filter = "RocketProfiler Sessions (.rocket)|*.rocket"
            };

            var result = dlg.ShowDialog();
            if (result.HasValue
                && result.Value)
            {
                _mainWindowViewModel.SessionFilePath = dlg.FileName;
            }
        }
    }
}
