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

            listener.Init(endPoint, SessionManager.Instance.CreateSession);

            JobTimer.Instance.Push(FlushRoom);
            
            while (true)
            {
                JobTimer.Instance.Flush();
            }
        }

        static void FlushRoom()
        {
            room.Push(() => room.Flush());
            JobTimer.Instance.Push(FlushRoom, 250);
        }
    }
}