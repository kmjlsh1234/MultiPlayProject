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
            /*
            S_RoomList packet = new S_RoomList();
            
            Dictionary<int, Room> dic = Program.roomManager.GetRoomDic();
            foreach (KeyValuePair<int, Room> pair in dic)
            {
                packet.roomList.Add(new S_RoomList.Room() { roomId = pair.Value.roomId, roomName = pair.Value.roomName });
            }

            Send(packet.Write());
            */
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"Client [{endPoint}] DisConnected!");
            SessionManager.Instance.Remove(this);
            if(room != null)
            {
                Room copyRoom = room;
                copyRoom.Push(() => copyRoom.ExitRoom(this, copyRoom.roomId));
                room = null;
            }
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }
    }
}
