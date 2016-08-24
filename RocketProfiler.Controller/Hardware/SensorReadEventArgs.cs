using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketProfiler.Controller.Hardware
{
    public class SensorReadEventArgs
    {
        public SensorReadEventArgs(string errorMessage, DateTime timestamp)
        {
            IsSuccessfulRead = false;
            Value = null;
            Timestamp = timestamp;
            ErrorMessage = errorMessage;
        }

        public SensorReadEventArgs(double value, DateTime timestamp)
        {
            IsSuccessfulRead = true;
            Value = value;
            Timestamp = timestamp;
            ErrorMessage = null;
        }

        public bool IsSuccessfulRead { get; private set; }
        public double? Value { get; private set; }
        public string ErrorMessage { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}
