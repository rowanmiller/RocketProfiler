using OxyPlot.Wpf;
using RocketProfiler.UI.ViewModels;

namespace RocketProfiler.UI.Views
{
    public abstract class PlotView : Plot
    {
        public PlotView(PlotViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public PlotViewModel ViewModel { get; private set; }
    }
}
