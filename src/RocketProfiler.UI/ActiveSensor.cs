using RocketProfiler.Controller.Hardware;
using RocketProfiler.UI.Views;
using System.Windows.Controls;

namespace RocketProfiler.UI
{
    public class ActiveSensor
    {
        public Sensor Sensor { get; set; }
        public UserControl StatusView { get; set; }
        public PlotView PlotView { get; set; }
    }
}
