using System.IO.Ports;
using System.Threading;

namespace RocketProfiler.Controller
{
    /// <summary>
    ///     Extension methods to interact with Numato GPIO module.
    ///     http://numato.com/16-channel-usb-gpio-module-with-analog-inputs/
    /// </summary>
    public static class NumatoExtensions
    {
        /// <summary>
        ///     The time (ms) to allow the IO module to process a command
        /// </summary>
        private static readonly int OPERATION_PAUSE = 1;

        /// <summary>
        ///     The voltage at which an analog pin will report 1023.
        ///     High voltage is listed as 3.3v but should be measured to get exact voltage
        ///     Measure it by setting a digital pin to 1 and measuring the voltage it is set to.
        /// </summary>
        private static readonly double HIGH_VOLTAGE = 3.1;

        /// <summary>
        ///     Sets the value on a digital pin of the Numato GPIO module.
        /// </summary>
        /// <param name="port">
        ///     The name of the COM port that the module is connected to.
        /// </param>
        /// <param name="pin">
        ///     The digital pin to set.
        /// </param>
        /// <param name="value">
        ///     The value to be set, should be 1 or 0.
        /// </param>
        public static void SetPin(this SerialPort port, int pin, int value)
        {
            if (value == 0)
            {
                port.Write($"gpio clear {pin}\r");
            }
            else
            {
                port.Write($"gpio set {pin}\r");
            }

            Thread.Sleep(OPERATION_PAUSE);
        }

        /// <summary>
        ///     Gets the value from a digital pin of the Numato GPIO module.
        /// </summary>
        /// <param name="port">
        ///     The name of the COM port that the module is connected to.
        /// </param>
        /// <param name="pin">
        ///     The digital pin to read.
        /// </param>
        /// <returns>
        ///     1 if the pin has a high voltage set, otherwise 0.
        /// </returns>
        public static int ReadDigitalPin(this SerialPort port, int pin)
        {
            port.DiscardInBuffer();
            port.Write($"gpio read {pin}\r");
            Thread.Sleep(OPERATION_PAUSE);
            var responseString = port.ReadExisting();

            return int.Parse(responseString.Substring(13, 1));
        }

        /// <summary>
        ///     Gets the voltage currently set on an analog pin of the Numato GPIO module.
        /// </summary>
        /// <param name="port">
        ///     The name of the COM port that the module is connected to.
        /// </param>
        /// <param name="pin">
        ///     The analog pin to read the voltage from.
        /// </param>
        /// <returns>
        ///     The voltage present on the pin.
        /// </returns>
        public static double ReadAnalogPin(this SerialPort port, int pin)
        {
            port.DiscardInBuffer();
            port.Write($"adc read {pin}\r");
            Thread.Sleep(OPERATION_PAUSE);
            var responseString = port.ReadExisting();

            responseString = responseString.Remove(0, 11);
            responseString = responseString.TrimEnd(responseString[responseString.Length - 1]);
            var response = double.Parse(responseString);

            // Result is 0-1023 representing range of 0v thru high voltage of IO board
            var volts = response / 1023.0 * HIGH_VOLTAGE;
            return volts;
        }
    }
}
