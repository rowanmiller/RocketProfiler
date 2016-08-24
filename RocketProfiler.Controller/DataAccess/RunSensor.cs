using RocketProfiler.Controller.Hardware;
using System.Collections.Generic;

namespace RocketProfiler.Controller.DataAccess
{
    public class RunSensor
    {
        public RunSensor()
        { }

        public RunSensor(Sensor sensor)
        {
            Title = sensor.Title;
            Units = sensor.Units;
            MinValue = sensor.MinValue;
            MaxValue = sensor.MaxValue;
            Threshold = sensor.Threshold;
        }

        public int Id { get; set; }
        public string Title { get; protected set; }
        public string Units { get; protected set; }
        public double MinValue { get; protected set; }
        public double MaxValue { get; protected set; }
        public double Threshold { get; protected set; }

        public int RunId { get; set; }
        public Run Run { get; set; }

        public ICollection<SensorValue> Values { get; set; } = new List<SensorValue>();
    }
}
