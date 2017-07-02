using System.Net.Sockets;
using System.Net;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventEmitter
{
    class Program
    {
        static UDPHandler udp;
        static void Main(string[] args)
        {

            udp = new UDPHandler("127.0.0.1", 41181, 41182, ReceiveEvent);

            TestEvent e = new TestEvent(1, (data) => EmitEvent(data));

            Console.ReadLine();
        }

        // test event
        static int emitCounter = 0;
        static void EmitEvent(int data)
        {
            string json = "{ type: \"event\", id: " + emitCounter.ToString() + ", data: " + data.ToString() + " }";
            Console.WriteLine("Sending => " + json);
            udp.SendData(json);
            emitCounter++;
        }

        // event data received
        static void ReceiveEvent(string data)
        {
            Console.WriteLine("Received => " + data);
        }
    }
}
