// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows.Controls;
using RocketProfiler.UI.ViewModels;

namespace RocketProfiler.UI.Views
{
    /// <summary>
    ///     Interaction logic for TemperatureSensorWidget.xaml
    /// </summary>
    public partial class TemperatureSensorWidget : UserControl
    {
        public TemperatureSensorWidget(TemperatureSensorWidgetViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
