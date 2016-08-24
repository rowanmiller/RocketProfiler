using RocketProfiler.Controller.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketProfiler.Controller.DataAccess
{
    public class SensorValue
    {
        public SensorValue()
        { }

        public SensorValue(SensorReadEventArgs args)
        {
            SuccessfulRead = args.IsSuccessfulRead;
            Value = args.Value;
            ErrorMessage = args.ErrorMessage;
            Timestamp = args.Timestamp;
        }

        public int Id { get; set; }
        public bool SuccessfulRead { get; private set; }
        public double? Value { get; private set; }
        public string ErrorMessage { get; private set; }
        public DateTime Timestamp { get; private set; }

        public int SensorInfoId { get; set; }
        public RunSensor SensorInfo { get; set; }
    }
}
