using OxyPlot.Axes;
using OxyPlot.Wpf;
using RocketProfiler.UI.ViewModels;
using System.Windows;
using System.Windows.Media;

namespace RocketProfiler.UI.Views
{
    public class TemperaturePlotView : PlotView
    {
        public TemperaturePlotView(TemperaturePlotViewModel viewModel)
            :base(viewModel)
        {
            Axes.Add(new OxyPlot.Wpf.TimeSpanAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "mm:ss"
            });

            Series.Add(
                new LineSeries
                {
                    ItemsSource = viewModel.DataPoints
                });

            Series.Add(
                new LineSeries
                {
                    ItemsSource = viewModel.ThresholdValues,
                    Color = (Color)ColorConverter.ConvertFromString("Red")
                });

            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "DataPoints")
                {
                    Application.Current.Dispatcher.InvokeAsync(() =>
                        InvalidatePlot());
                }
            };
        }
    }
}
