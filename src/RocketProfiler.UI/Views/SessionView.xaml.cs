// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows;
using RocketProfiler.UI.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;
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
            if (RunsGrid.Items.Count > 0)
            {
                RunsGrid.SelectedIndex = 0;
            }
        }

        public void CurrentRun_Changed(object sender, SelectionChangedEventArgs e)
        {
            //_sessionViewModel.CurrentRun = (Run)RunsGrid.SelectedItem;

            //for (var index = 0; index < _sessionViewModel.PlotWidgets.Count; index++)
            //{
            //    SensorGrid.RowDefinitions.Add(new RowDefinition
            //    {
            //        Height = new GridLength(300)
            //    });

            //    var plotWidget = _sessionViewModel.PlotWidgets[index];
            //    plotWidget.Padding = new Thickness(20);

            //    var plotBorder = new Border
            //    {
            //        Padding = new Thickness(3),
            //        Child = new Border
            //        {
            //            BorderThickness = new Thickness(1),
            //            BorderBrush = Brushes.Black,
            //            Child = plotWidget
            //        }
            //    };
                
            //    Grid.SetRow(plotBorder, index);
            //    Grid.SetColumn(plotBorder, 1);
            //    SensorGrid.Children.Add(plotBorder);
            //}
        }
    }
}
