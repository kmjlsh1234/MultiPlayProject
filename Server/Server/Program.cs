using System;
using System.Net;
using System.Net.Sockets;
using ServerCore;
using Google.Protobuf.Protocol;

namespace Server
{
    public class Program
    {
        static Listener listener = new Listener();
        public static RoomManager roomManager = new RoomManager();

        
        
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

            foreach (KeyValuePair<int, Room> pair in roomManager.GetRoomDic())
            {
                pair.Value.Push(() => pair.Value.Flush());
            }
            JobTimer.Instance.Push(FlushRoom, 250);
        }
    }
}