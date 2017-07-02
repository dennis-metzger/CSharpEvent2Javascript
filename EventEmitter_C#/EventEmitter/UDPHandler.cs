using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventEmitter
{
    class UDPHandler
    {
        UdpClient sender;
        UdpClient receiver;

        IPEndPoint fromEP;
        IPEndPoint toEP;

        Action<string> dataReceivedCallback;

        public UDPHandler(string sendIP, int sendPort)
        {
            toEP = new IPEndPoint(IPAddress.Parse(sendIP), sendPort);

            sender = new UdpClient();
            sender.Connect(toEP);
            sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public UDPHandler(string sendIP, int sendPort, int listenPort, Action<string> dataReceivedCallback)
        {
            this.dataReceivedCallback = dataReceivedCallback;

            toEP = new IPEndPoint(IPAddress.Parse(sendIP), sendPort);
            fromEP = new IPEndPoint(0, listenPort);

            sender = new UdpClient();
            sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            sender.Connect(toEP);

            receiver = new UdpClient();
            receiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            receiver.Client.Bind(fromEP);
            receiver.BeginReceive(onReceive, null);
        }

        public void SendData(string data)
        {
            byte[] byteData =  Encoding.ASCII.GetBytes(data);
            sender.Send(byteData, byteData.Length);
        }

        protected void onReceive(IAsyncResult r)
        {
            string data = Encoding.ASCII.GetString(receiver.EndReceive(r, ref fromEP));
            receiver.BeginReceive(onReceive, null);
            dataReceivedCallback(data);
        }
    }
}
