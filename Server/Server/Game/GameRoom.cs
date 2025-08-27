using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class GameRoom
    {
        object key = new object();
        public int roomId { get; set; }
        public int masterId { get; set; }
        public string roomName { get; set; }

        public bool isPublic { get; set; } = false;

        public ushort playerCount { get; set; } = 0;

        Dictionary<int, ClientSession> sessions = new Dictionary<int, ClientSession>();

        public void EnterGame(ClientSession session)
        {
            
            lock (key)
            {
                sessions.Add(session.sessionId, session);

                //본인에게 정보 전송
                S_Roominfo roomInfoPacket = new S_Roominfo()
                {
                    RoomId = roomId,
                    MasterId = masterId,
                    RoomName = roomName,
                };
                
                foreach(ClientSession s in sessions.Values)
                {
                    PlayerInfo playerInfo = new PlayerInfo()
                    {
                        SessionId = s.sessionId,
                        IsReady = false
                    };
                    roomInfoPacket.Players.Add(playerInfo);
                }
                session.Send(roomInfoPacket);

                //타인에게 정보 전송
                S_Enterroom enterRoomPacket = new S_Enterroom()
                {
                    SessionId = session.sessionId,
                    IsReady = false,
                };

                BroadCast(enterRoomPacket);
                Console.WriteLine($"Player {session.sessionId} Enter Room {roomId}");
            }
            
        }

        public void LeaveGame(int sessionId)
        {
            lock (key)
            {
                ClientSession session = null;
                if(sessions.TryGetValue(sessionId, out session))
                {
                    sessions.Remove(sessionId);
                    session.room = null;

                    //Room에 BroadCast
                    S_Exitroom leavePacket = new S_Exitroom()
                    {
                        SessionId = session.sessionId,
                    };
                    BroadCast(leavePacket);
                }
            }
        }

        public void BroadCast(IMessage packet)
        {
            lock (key)
            {
                foreach (ClientSession session in sessions.Values)
                {
                    session.Send(packet);
                }
            }
            
        }

    }
}
