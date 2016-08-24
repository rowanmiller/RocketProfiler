// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RocketProfiler.Controller.Hardware
{
    public abstract class Sensor
    {
        public Sensor(string title, string units, double minValue, double maxValue, double threshold)
        {
            Title = title;
            Units = units;
            MinValue = minValue;
            MaxValue = maxValue;
            Threshold = threshold;
        }

        public string Title { get; protected set; }
        public string Units { get; protected set; }
        public double MinValue { get; protected set; }
        public double MaxValue { get; protected set; }
        public double Threshold { get; protected set; }

        public delegate void SensorReadEventHandler(object sender, SensorReadEventArgs e);

        public event SensorReadEventHandler SensorReadEvent;

        protected virtual void RaiseSampleEvent(SensorReadEventArgs args)
        {
            SensorReadEvent?.Invoke(this, args);
        }
    }
}
