// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace RocketProfiler.Controller.Hardware
{
    /// <summary>
    ///     Reads temperature from the AD8945 analog thermocouple amplifier.
    ///     https://www.adafruit.com/products/1778
    /// </summary>
    public class AD8495TemperatureSensor : Sensor
    {
        private readonly int _pin;

        /// <summary>
        ///     Initializes a new instance of the AD8495TemperatureSensor class.
        /// </summary>
        /// <param name="gpioModule">
        ///     The GPIO module that the sensor is hooked to
        /// </param>
        /// <param name="pin">
        ///     Output from the AD8945 (input to the micro-controller) which sets a voltage to indicate the current temperature.
        ///     Pin is labeled OUT on hardware.
        /// </param>
        public AD8495TemperatureSensor(string title, double threshold, GpioModule gpioModule, int pin)
            :base(title: title, units: "°C", minValue: 0, maxValue: 400, threshold: threshold)
        {
            _pin = pin;

            gpioModule.QueueRepeatingWork(g => RaiseSampleEvent(ReadValue(g)));
        }

        public SensorReadEventArgs ReadValue(GpioModule gpioModule)
        {
            // Temperature (°C) = (volts - 1.25) / 5mv
            var time = DateTime.Now;
            try
            {
                var volts = gpioModule.ReadAnalogPin(_pin);
                var temperature = (volts - 1.25) / 0.005;

                return new SensorReadEventArgs(temperature, time);
            }
            catch (Exception ex)
            {
                return new SensorReadEventArgs($"I/O Failure: {ex.GetType()} {ex.Message}", time);
            }
        }
    }
}
