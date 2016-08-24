// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace RocketProfiler.Controller.Hardware
{
    /// <summary>
    ///     Reads temperature from the MAX6675 digital thermocouple amplifier.
    ///     https://www.adafruit.com/products/269
    /// </summary>
    public class MAX6675TemperatureSensor : Sensor
    {
        private readonly int _clockPin;
        private readonly int _chipSelectPin;
        private readonly int _dataOutputPin;

        /// <summary>
        ///     Initializes a new instance of the AD8495TemperatureSensor class.
        /// </summary>
        /// <param name="gpioModule">
        ///     The GPIO module that the sensor is hooked to
        /// </param>
        /// <param name="clockPin">
        ///     Input to the MAX6675 (output from micro-controller) which indicates when to present another bit of data.
        ///     Pin is labeled SCK on hardware.
        /// </param>
        /// <param name="chipSelectPin">
        ///     input to the MAX6675 (output from the micro-controller) which tells the chip when its time to read the
        ///     thermocouple and output more data.
        ///     Pins is labeled CS on hardware.
        /// </param>
        /// <param name="dataOutputPin">
        ///     Output from the MAX6675 (input to the micro-controller) which carries each bit of data.
        ///     Pin is labeled 90 on hardware.
        /// </param>
        public MAX6675TemperatureSensor(string title, double threshold, GpioModule gpioModule, int clockPin, int chipSelectPin, int dataOutputPin)
            :base(title: title, units: "°C", minValue: 0, maxValue: 1024, threshold: threshold)
        {
            _clockPin = clockPin;
            _chipSelectPin = chipSelectPin;
            _dataOutputPin = dataOutputPin;

            gpioModule.QueueRepeatingWork(g => RaiseSampleEvent(ReadValue(g)));
        }

        public SensorReadEventArgs ReadValue(GpioModule gpioModule)
        {
            var time = DateTime.Now;
            try
            {
                gpioModule.SetPin(_chipSelectPin, 0);
                var result = ReadTwoBytes(gpioModule);
                gpioModule.SetPin(_chipSelectPin, 1);

                // Three lest significant bits are flags and not part of reading
                // Third least significant flags that thermocouple is detached
                if ((result & 4) != 0)
                {
                    return new SensorReadEventArgs("Thermocouple not attached", time);
                }

                // Discard the flag bits and calculate result
                result >>= 3;
                var temperature = result * 0.25;

                return new SensorReadEventArgs(temperature, time);
            }
            catch (Exception ex)
            {
                return new SensorReadEventArgs($"I/O Failure: {ex.GetType()} {ex.Message}", time);
            }
        }

        private int ReadTwoBytes(GpioModule gpioModule)
        {
            // Two bytes of data is streamed, data pin is set to next bit each time clock pin is cycled
            var result = 0;
            for (var i = 15; i >= 0; i--)
            {
                gpioModule.SetPin(_clockPin, 0);

                // Bits are streamed with most significant first
                var bit = gpioModule.ReadDigitalPin(_dataOutputPin);
                result |= (bit << i);

                gpioModule.SetPin(_clockPin, 1);
            }

            return result;
        }
    }
}
