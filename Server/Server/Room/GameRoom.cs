using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Game;
using Server.Game.Map;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoom : Room<ClientSession>
    {
        public Dictionary<int, GamePlayer> players = new Dictionary<int, GamePlayer>();
        public Map map { get; set; } = new Map();
        public int count = 0;
        

        #region :::: Abstract Function
        public override void BroadCast(IMessage packet)
        {
            foreach (GamePlayer player in players.Values)
            {
                player.session.Send(packet);
            }
        }

        public override void EnterRoom(ClientSession session)
        {
            GamePlayer player = new GamePlayer(session, session.sessionId);
            players.Add(player.session.sessionId, player);

            //나에게 정보 전송
            Console.WriteLine($"Session {session.sessionId} Enter GameRoom");
            //우리팀에게 브로드캐스트

            if (count == players.Count)
            {
                S_Loadingstart packet = new S_Loadingstart();
                BroadCast(packet);
            }
        }

        public override void ExitRoom(ClientSession session)
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
            Console.WriteLine($"Session {session.sessionId} leave Game Room");
        }
        #endregion

        public bool Init(int count)
        {
            this.count = count;
            map.LoadMap("MapData");
            return true;
        }

        public void Update()
        {
            Flush();
            Console.WriteLine("GameRoom Update");
        }


        public void HandleMove(ClientSession session, C_Move packet)
        {
            GamePlayer player = null;
            players.TryGetValue(session.sessionId, out player);
            if (player != null)
            {
                player.objectinfo.Pos.PosX = packet.PosX;
                player.objectinfo.Pos.PosY = packet.PosY;
                player.objectinfo.Pos.PosZ = packet.PosZ;
                player.objectinfo.RotY = packet.RotY;
                player.objectinfo.State = packet.State;
                S_Move movePacket = new S_Move()
                {
                    SessionId = session.sessionId,
                    PosX = packet.PosX,
                    PosY = packet.PosY,
                    PosZ = packet.PosZ,
                    RotY = packet.RotY,
                    State = packet.State,
                };
                BroadCast(movePacket);
                Console.WriteLine($"{session.sessionId} : [ {movePacket.PosX}, {movePacket.PosY}, {movePacket.PosZ}]");
            }
        }

        public void SpawnEnemy()
        {
        }
    }
}
