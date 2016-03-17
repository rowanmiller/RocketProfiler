// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RocketProfiler.UI.ViewModels;
using System.Windows;

namespace RocketProfiler.UI.Views
{
    /// <summary>
    /// Interaction logic for UploadView.xaml
    /// </summary>
    public partial class UploadView : Window
    {
        public UploadView(UploadViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
