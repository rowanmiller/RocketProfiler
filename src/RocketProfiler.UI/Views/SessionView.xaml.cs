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

            Loaded += SessionView_Loaded;
        }

        private void SessionView_Loaded(object sender, RoutedEventArgs e)
        {
            if (RunsList.Items.Count > 0)
            {
                RunsList.SelectedIndex = 0;
            }
        }

        public void CurrentRun_Changed(object sender, SelectionChangedEventArgs e)
        {
            _sessionViewModel.CurrentRun = (Run)RunsList.SelectedItem;

            for (var index = 0; index < _sessionViewModel.PlotWidgets.Count; index++)
            {
                SensorGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(300)
                });

                Grid.SetRow(_sessionViewModel.PlotWidgets[index], index);
                Grid.SetColumn(_sessionViewModel.PlotWidgets[index], 1);
                SensorGrid.Children.Add(_sessionViewModel.PlotWidgets[index]);
            }
        }
    }
}
