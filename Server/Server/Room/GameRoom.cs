using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoom : Room<ClientSession>
    {
        public Dictionary<int, GamePlayer> players = new Dictionary<int, GamePlayer>();
        public int count = 0;
        #region :::: Abstract Function
        public override void BroadCast(IMessage packet)
        {
            lock (key)
            {
                foreach (GamePlayer player in players.Values)
                {
                    player.session.Send(packet);
                }
            }
        }

        public override void EnterRoom(ClientSession session)
        {
            lock (key)
            {
                GamePlayer player = new GamePlayer()
                {
                    session = session,
                    position = new Vector3(0f, 1f, 0f),
                    rotY = 0,
                };
                players.Add(player.session.sessionId, player);

                //나에게 정보 전송
                Console.WriteLine($"Session {session.sessionId} Enter GameRoom");
                //우리팀에게 브로드캐스트

                if(count == players.Count)
                {
                    S_Loadingstart packet = new S_Loadingstart();
                    BroadCast(packet);
                }
            }
        }

        public override void ExitRoom(ClientSession session)
        {
            lock (key)
            {
                players.Remove(session.sessionId);
                session.gameRoom = null;

                if (players.Count == 0)
                {
                    RoomManager.Instance.RemoveGameRoom(roomId);
                }
                else
                {
                    if (masterId == session.sessionId)
                    {
                        int originMasterId = masterId;
                        masterId = players.First().Value.session.sessionId;
                        S_Changeroominfo changeRoomInfoPacket = new S_Changeroominfo()
                        {
                            RoomId = roomId,
                            MasterId = masterId,
                        };
                        BroadCast(changeRoomInfoPacket);
                        Console.WriteLine($"Master Change {originMasterId} -> {masterId}");
                    }
                }

                //Room에 BroadCast
                S_Exitgameroom exitRoomPacket = new S_Exitgameroom()
                {
                    SessionId = session.sessionId,
                };
                BroadCast(exitRoomPacket);
            }
            Console.WriteLine($"Session {session.sessionId} leave Game Room");
        }
        #endregion

        public bool Init(int count)
        {
            this.count = count;
            return true;
        }

        public void HandleMove(ClientSession session, C_Move packet)
        {
            lock(key)
            {
                GamePlayer player = null;
                players.TryGetValue(session.sessionId, out player);
                if (player != null)
                {
                    player.position = new Vector3(packet.PosX, packet.PosY, packet.PosZ);
                    player.rotY = packet.RotY;

                    S_Move movePacket = new S_Move()
                    {
                        SessionId = session.sessionId,
                        PosX = packet.PosX,
                        PosY = packet.PosY,
                        PosZ = packet.PosZ,
                        RotY = packet.RotY,
                    };
                    BroadCast(movePacket);
                    Console.WriteLine($"{session.sessionId} : [ {movePacket.PosX}, {movePacket.PosY}, {movePacket.PosZ}]");
                }
            }
        }

        public void HandleInput(ClientSession session, C_Input packet)
        {
            lock (key)
            {
                GamePlayer player = null;
                players.TryGetValue(session.sessionId, out player);
                if (player != null)
                {
                    float deltaTime = 0.02f; // 클라이언트 sendInterval과 맞춤
                    Vector3 dir = new Vector3(packet.DirX, 0, packet.DirY);
                    if (dir.LengthSquared() > 0.001f)
                    {
                        dir = Vector3.Normalize(dir);
                        player.position += dir * player.moveSpeed * deltaTime;
                        player.rotY = (float)(Math.Atan2(dir.X, dir.Z) * (180 / Math.PI));
                    }

                    S_Move resPacket = new S_Move()
                    {
                        SessionId = session.sessionId,
                        PosX = player.position.X,
                        PosY = player.position.Y,
                        PosZ = player.position.Z,
                        RotY = player.rotY,
                    };
                    BroadCast(resPacket);
                    Console.WriteLine($"{session.sessionId} : [ {resPacket.PosX}, {resPacket.PosY}, {resPacket.PosZ}]");
                }
            }
            
        }

    }
}
