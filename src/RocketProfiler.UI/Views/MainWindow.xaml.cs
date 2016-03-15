// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = mainWindowViewModel;

            for (var index = 0; index < mainWindowViewModel.SensorWidgets.Count; index++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition());

                Grid.SetRow(mainWindowViewModel.SensorWidgets[index], index);
                Grid.SetColumn(mainWindowViewModel.SensorWidgets[index], 0);
                MainGrid.Children.Add(mainWindowViewModel.SensorWidgets[index]);

                Grid.SetRow(mainWindowViewModel.PlotWidgets[index], index);
                Grid.SetColumn(mainWindowViewModel.PlotWidgets[index], 1);
                MainGrid.Children.Add(mainWindowViewModel.PlotWidgets[index]);
            }
        }
    }
}
