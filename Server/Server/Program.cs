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
        static List<System.Timers.Timer> timers = new List<System.Timers.Timer>();

        public static void TickRoom(GameRoom room, int tick = 100)
        {
            var timer = new System.Timers.Timer();
            timer.Interval = tick;
            timer.Elapsed += ((s,e) => { room.Update(); });
            timer.AutoReset = true;
            timer.Enabled = true;

            timers.Add(timer);
        }


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