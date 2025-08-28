using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
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
                };
                
                foreach(ClientSession s in sessions.Values)
                {
                    PlayerInfo playerInfo = new PlayerInfo()
                    {
                        SessionId = s.sessionId,
                        NickName = s.nickName,
                        IsReady = false
                    };
                    roomInfoPacket.Players.Add(playerInfo);
                }
                session.Send(roomInfoPacket);

                //타인에게 정보 전송
                S_Enterroom enterRoomPacket = new S_Enterroom()
                {
                    PlayerInfo = new PlayerInfo()
                    {
                        SessionId= session.sessionId,
                        NickName= session.nickName,
                        IsReady = false
                    }
                };

                BroadCast(enterRoomPacket);
                Console.WriteLine($"Player {session.sessionId} Enter Room {roomId}");
            }
            
        }

        public void LeaveGame(ClientSession session)
        {
            lock (key)
            {
                sessions.Remove(session.sessionId);
                session.room = null;

                if(sessions.Count == 0)
                {
                    RoomManager.Instance.RemoveRoom(roomId);
                }
                else
                {
                    if (masterId == session.sessionId)
                    {
                        masterId = sessions.First().Value.sessionId;
                        S_Changeroominfo changeRoomInfoPacket = new S_Changeroominfo()
                        {
                            RoomId = roomId,
                            MasterId = masterId,
                        };
                        BroadCast(changeRoomInfoPacket);
                        Console.WriteLine($"Master Change {masterId} -> {}");
                    }
                }

                //Room에 BroadCast
                S_Exitroom leavePacket = new S_Exitroom()
                {
                    SessionId = session.sessionId,
                };
                BroadCast(leavePacket);
            }
            Console.WriteLine($"Session {session.sessionId} leave Room");
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
