// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RocketProfiler.Controller
{
    public abstract class Sensor
    {
        protected Sensor(string name, string units, double maxValue)
        {
            Info = new SensorInfo { Name = name, Units = units, MaxValue = maxValue };
        }

        public virtual SensorValue ReadValue()
        {
            LastRead.Value = DoRead();
            return LastRead.Value;
        }

        public abstract SensorValue DoRead();

        public CurrentSensorValue LastRead { get; } = new CurrentSensorValue();

        public SensorInfo Info { get; }
    }
}
