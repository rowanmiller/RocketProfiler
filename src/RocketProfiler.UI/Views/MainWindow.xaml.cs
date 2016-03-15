﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows;
using System.Windows.Controls;
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
            _mainWindowViewModel.RunController.StartRecoding(RunName.Text, RunDescription.Text);
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.RunController.StopRecording();
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }
    }
}
