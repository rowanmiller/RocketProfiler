// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RocketProfiler.Controller.Hardware
{
    public class GpioRelay
    {
        private readonly string _name;
        private readonly GpioModule _gpio;
        private readonly int _pin;
        private readonly bool _lowIsOn;

        public GpioRelay(string name, GpioModule gpio, int pin, bool lowIsOn)
        {
            _name = name;
            _gpio = gpio;
            _pin = pin;
            _lowIsOn = lowIsOn;

            TurnOff();
        }

        public void TurnOn()
        {
            _gpio.QueueHighPriorityWork(gpio => gpio.SetPin(_pin, _lowIsOn ? 0 : 1));
        }

        public void TurnOff()
        {
            _gpio.QueueHighPriorityWork(gpio => gpio.SetPin(_pin, _lowIsOn ? 1 : 0));
        }
    }
}
