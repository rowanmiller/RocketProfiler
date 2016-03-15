// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RocketProfiler.Controller
{
    public abstract class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual SensorValue ReadValue()
        {
            LastRead.Value = DoRead();
            return LastRead.Value;
        }

        public abstract SensorValue DoRead();

        public CurrentSensorValue LastRead { get; } = new CurrentSensorValue();
    }
}
