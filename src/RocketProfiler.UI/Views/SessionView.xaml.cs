// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RocketProfiler.UI.ViewModels;
using System.Windows.Controls;

namespace RocketProfiler.UI.Views
{
    /// <summary>
    /// Interaction logic for SessionView.xaml
    /// </summary>
    public partial class SessionView : Page
    {
        public SessionView(SessionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
