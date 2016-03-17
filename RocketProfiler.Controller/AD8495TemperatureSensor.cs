// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO.Ports;

namespace RocketProfiler.Controller
{
    /// <summary>
    ///     Reads temperature from the AD8945 analog thermocouple amplifier.
    ///     https://www.adafruit.com/products/1778
    /// </summary>
    public class AD8495TemperatureSensor : Sensor
    {
        private readonly SerialPort _port;
        private readonly int _pin;

        /// <summary>
        ///     Initializes a new instance of the AD8495TemperatureSensor class.
        /// </summary>
        /// <param name="port">
        ///     Name of serial port that the module is hooked to
        /// </param>
        /// <param name="pin">
        ///     Output from the AD8945 (input to the microcontroller) which sets a voltage to indicate the current temperature.
        ///     Pin is labeled OUT on hardware.
        /// </param>
        public AD8495TemperatureSensor(string name, SerialPort port, int pin)
        {
            Name = name;
            _port = port;
            _pin = pin;
        }

        public override string Units => "Degrees Celsius";

        public override double MaxValue => 400;

        public override SensorValue DoRead()
        {
            // Temperature (°C) = (volts - 1.25) / 5mv
            var time = DateTime.Now;
            try
            {
                var volts = _port.ReadAnalogPin(_pin);
                var temperature = (volts - 1.25) / 0.005;

                return new SensorValue
                {
                    Sensor = this,
                    Value = temperature,
                    Timestamp = time
                };
            }
            catch (Exception ex)
            {
                return new ErrorSensorValue
                {
                    Sensor = this,
                    Timestamp = time,
                    ErrorMessage = $"I/O Failure: {ex.GetType()} {ex.Message}",
                    Value = null
                };
            }
        }
    }
}
