using System;
using System.IO.Ports;

namespace RocketProfiler.Controller
{
    /// <summary>
    ///     Reads temperature from the MAX6675 digital thermocouple amplifier.
    ///     https://www.adafruit.com/products/269
    /// </summary>
    public class MAX6675TemperatureSensor : Sensor
    {
        private readonly SerialPort _port;
        private readonly int _clockPin;
        private readonly int _chipSelectPin;
        private readonly int _dataOutputPin;

        /// <summary>
        ///     Initializes a new instance of the AD8495TemperatureSensor class.
        /// </summary>
        /// <param name="port">
        ///     Name of serial port that the module is hooked to
        /// </param>
        /// <param name="clockPin"> 
        ///     Input to the MAX6675 (output from microcontroller) which indicates when to present another bit of data.
        ///     Pin is labeled SCK on hardware. 
        /// </param>
        /// <param name="chipSelectPin">
        ///     input to the MAX6675 (output from the microcontroller) which tells the chip when its time to read the 
        ///     thermocouple and output more data. 
        ///     Pins is labeled CS on hardware.
        /// </param>
        /// <param name="dataOutputPin">
        ///     Output from the MAX6675 (input to the microcontroller) which carries each bit of data.
        ///     Pin is labeled 90 on hardware. 
        /// </param>
        public MAX6675TemperatureSensor(string name, SerialPort port, int clockPin, int chipSelectPin, int dataOutputPin)
        {
            Name = name;
            _port = port;
            _clockPin = clockPin;
            _chipSelectPin = chipSelectPin;
            _dataOutputPin = dataOutputPin;
        }

        public override string Units => "Degrees Celsius";

        public override double MaxValue => 1024;

        public override SensorValue DoRead()
        {
            var time = DateTime.Now;
            try
            {
                _port.SetPin(_chipSelectPin, 0);
                int result = ReadTwoBytes();
                _port.SetPin(_chipSelectPin, 1);

                // Three lest significant bits are flags and not part of reading
                // Third least significant flags that thermocouple is detached
                if ((result & 4) != 0)
                {
                    return new ErrorSensorValue
                    {
                        Timestamp = time,
                        ErrorMessage = "Thermocouple not attached",
                        Value = null
                    };
                }

                // Discard the flag bits and calcuate result
                result >>= 3;
                var temperature = result * 0.25;

                return new SensorValue
                {
                    Value = temperature,
                    Timestamp = time
                };
            }
            catch (Exception ex)
            {
                return new ErrorSensorValue
                {
                    Timestamp = time,
                    ErrorMessage = $"I/O Failure: {ex.GetType()} {ex.Message}",
                    Value = null
                };
            }
        }

        private int ReadTwoBytes()
        {
            // Two bytes of data is streamed, data pin is set to next bit each time clock pin is cycled
            int result = 0;
            for (int i = 15; i >= 0; i--)
            {
                _port.SetPin(_clockPin, 0);

                // Bits are streamed with most significant first
                var bit = _port.ReadDigitalPin(_dataOutputPin);
                result |= (bit << i);

                _port.SetPin(_clockPin, 1);
            }

            return result;
        }
    }
}
