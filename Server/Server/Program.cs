using System;
using System.Net;
using System.Net.Sockets;
using ServerCore;

namespace Server
{
    public class Program
    {
        static Listener listener = new Listener();
        public static Room room = new Room();
        
        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 8888);

            listener.Init(endPoint, () => { return new ClientSession(); } );
            
            while (true)
            {

            }
        }
    }
}