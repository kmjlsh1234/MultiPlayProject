using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientSession : Session
    {
        public int sessionId { get; set; }

        public Room room { get; set; }
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"Client [{endPoint}] Connected!");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"Client [{endPoint}] DisConnected!");
            SessionManager.Instance.Remove(this);
            if(room != null)
            {
                Room copyRoom = room;
                copyRoom.Push(() => copyRoom.ExitRoom(this));
                room = null;
            }
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }
    }
}
