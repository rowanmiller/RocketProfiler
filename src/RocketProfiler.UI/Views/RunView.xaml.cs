// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using RocketProfiler.UI.ViewModels;

namespace RocketProfiler.UI.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RunView : Page
    {
        private readonly RunViewModel _mainWindowViewModel;

        public RunView(RunViewModel mainWindowViewModel)
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

                var plotWidget = mainWindowViewModel.PlotWidgets[index];
                plotWidget.Padding = new Thickness(20);

                var plotBorder = new Border
                {
                    Padding = new Thickness(3),
                    Child = new Border
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Black,
                        Child = plotWidget
                    }
                };

                Grid.SetRow(plotBorder, index);
                Grid.SetColumn(plotBorder, 1);
                SensorGrid.Children.Add(plotBorder);
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
