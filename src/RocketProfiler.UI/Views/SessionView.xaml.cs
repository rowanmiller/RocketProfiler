// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows;
using RocketProfiler.UI.ViewModels;
using System.Windows.Controls;
using RocketProfiler.Controller;

namespace RocketProfiler.UI.Views
{
    /// <summary>
    /// Interaction logic for SessionView.xaml
    /// </summary>
    public partial class SessionView : Page
    {
        private readonly SessionViewModel _sessionViewModel;

        public SessionView(SessionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            _sessionViewModel = viewModel;

            for (var index = 0; index < viewModel.PlotWidgets.Count; index++)
            {
                SensorGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(300)
                });

                Grid.SetRow(viewModel.PlotWidgets[index], index);
                Grid.SetColumn(viewModel.PlotWidgets[index], 1);
                SensorGrid.Children.Add(viewModel.PlotWidgets[index]);
            }
        }

        public void CurrentRun_Changed(object sender, SelectionChangedEventArgs e)
        {
            _sessionViewModel.CurrentRun = (Run)RunsList.SelectedItem;
        }
    }
}
