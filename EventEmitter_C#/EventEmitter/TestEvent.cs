using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEmitter
{
    class TestEvent
    {
        Random rnd;
        Timer t;
        Action<int> call;

        public TestEvent(int frequency, Action<int> callback)
        {
            rnd = new Random();
            t = new Timer();
            call = callback;

            t.Interval = frequency;
            t.AutoReset = true;
            t.Elapsed += Event;
            t.Start();
        }

        void Event(object sender, ElapsedEventArgs e)
        {
            int data = rnd.Next(0, int.MaxValue);
            call(data);
        }
    }
}
