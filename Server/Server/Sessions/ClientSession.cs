using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Game;
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
        public string nickName { get; set; }
        public MatchRoom matchRoom { get; set; }
        public GameRoom gameRoom { get; set; }

        public void Send(IMessage packet)
        {
            string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
            MsgId id = (MsgId) Enum.Parse(typeof(MsgId), msgName);

            ushort size = (ushort)packet.CalculateSize();
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes((ushort) (size + 4)), 0, sendBuffer, 0, sizeof(ushort));
            
            Array.Copy(BitConverter.GetBytes((ushort) id), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);

            Send(new ArraySegment<byte>(sendBuffer));
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"Client [{endPoint}] Connected!");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"Client [{endPoint}] DisConnected!");
            SessionManager.Instance.Remove(this);
            if(matchRoom != null)
            {
                MatchRoom copyRoom = matchRoom;
                copyRoom.Push(copyRoom.ExitRoom, this);
                matchRoom = null;
            }

            if(gameRoom != null)
            {
                GameRoom copyRoom = gameRoom;
                copyRoom.ExitRoom(this);
                gameRoom = null;
            }
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }
    }
}
