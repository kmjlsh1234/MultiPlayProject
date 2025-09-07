using System;
using System.Net;
using System.Net.Sockets;
using ServerCore;
using Google.Protobuf.Protocol;
using Google.Protobuf;
using Server.Game;

namespace Server
{
    public class Program
    {
        static Listener listener = new Listener();

        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 8888);

            listener.Init(endPoint, SessionManager.Instance.CreateSession);

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}