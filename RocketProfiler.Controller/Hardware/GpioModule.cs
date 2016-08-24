using System;
using System.Collections.Concurrent;
using System.Threading;

namespace RocketProfiler.Controller.Hardware
{
    public abstract class GpioModule
    {
        private readonly ConcurrentQueue<Action<GpioModule>> _work = new ConcurrentQueue<Action<GpioModule>>();
        private readonly ConcurrentQueue<Action<GpioModule>> _highPriorityWork = new ConcurrentQueue<Action<GpioModule>>();

        public abstract void SetPin(int pin, int value);
        public abstract int ReadDigitalPin(int pin);
        public abstract double ReadAnalogPin(int pin);

        public void QueueWork(Action<GpioModule> work)
        {
            _work.Enqueue(work);
        }

        public void QueueRepeatingWork(Action<GpioModule> work)
        {
            _work.Enqueue(g => WorkAndRequeue(work));
        }

        public void QueueHighPriorityWork(Action<GpioModule> work)
        {
            _highPriorityWork.Enqueue(work);
        }

        private void WorkAndRequeue(Action<GpioModule> work)
        {
            work(this);
            _work.Enqueue(g => WorkAndRequeue(work));
        }

        public void MonitorAndProcessQueue(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                Action<GpioModule> work;
                if (!_highPriorityWork.IsEmpty && _highPriorityWork.TryDequeue(out work))
                {
                    work(this);
                }
                else if (!_work.IsEmpty && _work.TryDequeue(out work))
                {
                    work(this);
                }

                // TODO Move to MAX6675 module as it doesn't handle super regular reads
                Thread.Sleep(500);
            }
        }
    }
}
