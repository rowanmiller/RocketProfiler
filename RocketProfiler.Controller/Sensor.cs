// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RocketProfiler.Controller
{
    public abstract class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public abstract SensorValue ReadValue();
    }
}
