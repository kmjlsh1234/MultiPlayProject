using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Game;
using Server.Game.Map;
using Server.Game.Object;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoom : Room
    {
        public Dictionary<int, GamePlayer> players = new Dictionary<int, GamePlayer>();
        public Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();

        public Map map { get; set; } = new Map();
        public int count = 0;
        public int loadingCompleteCount = 0;

        public GameRoom()
        {
            roomType = RoomType.Game;
        }

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
            GamePlayer player = new GamePlayer(session);
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
                RoomManager.Instance.RemoveRoom<GameRoom>(roomId, roomType);
            }
            else
            {
                if (masterId == session.sessionId)
                {
                    int originMasterId = masterId;
                    masterId = players.First().Value.session.sessionId;
                    //TODO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!마스터 바뀐것 처리
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

        public override void Update()
        {
            Flush();
            //Console.WriteLine("GameRoom Update");
        }
        #endregion

        public void CheckGameStart(ClientSession session)
        {
            loadingCompleteCount++;
            if (loadingCompleteCount.Equals(players.Count))
            {
                S_Gameroominfo packet = new S_Gameroominfo();
                
                Gameroominfo info = new Gameroominfo();
                info.RoomId = roomId;
                info.MasterId = masterId;
                
                foreach(GamePlayer gamePlayer in players.Values)
                {
                    info.Players.Add(gamePlayer.objectinfo);
                }

                packet.RoomInfo = info;

                BroadCast(packet);
                PushAfter(2000, SpawnEnemy);
            }
        }

        public bool Init(int count)
        {
            this.count = count;
            //map.LoadMap("MapData");
            
            return true;
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
                    SessionId = player.session.sessionId,
                    PosX = packet.PosX,
                    PosY = packet.PosY,
                    PosZ = packet.PosZ,
                    RotY = packet.RotY,
                    State = packet.State,
                };
                BroadCast(movePacket);
                Console.WriteLine($"{player.objectId} : [ {movePacket.PosX}, {movePacket.PosY}, {movePacket.PosZ}]");
            }
        }

        public void HandleEnemyMove(ClientSession session, C_Enemymove packet)
        {
            S_Enemymove resPacket = new S_Enemymove();
            Console.WriteLine($"EnemyInArea : {packet.Enemies.Count}");
            foreach(Objectinfo info in packet.Enemies)
            {
                Enemy enemy = null;
                enemies.TryGetValue(info.ObjectId, out enemy);
                if(enemy != null)
                {
                    enemy.objectinfo.Pos = info.Pos;
                    resPacket.Enemies.Add(info);
                }
            }

            BroadCast(resPacket);
        }

        public void SpawnEnemy()
        {

            try
            {
                Enemy enemy = ObjectManager.Instance.Add<Enemy>();
                enemy.objectinfo.ObjectId = enemy.objectId;
                
                enemies.Add(enemy.objectId, enemy);
                enemy.objectinfo.TargetId = FindeTargetPlayer(enemy.objectinfo.Pos);
                S_Spawnenemy packet = new S_Spawnenemy()
                { 
                    ObjectInfo = enemy.objectinfo
                };

                BroadCast(packet);
                Console.WriteLine($"Enemy {enemy.objectinfo.ObjectId} Spawn");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SpawnEnemy ERROR] {ex}");
            }
            finally
            {
                PushAfter(5000, SpawnEnemy);
            }
        }

        public int FindeTargetPlayer(Positioninfo info)
        {
            GamePlayer closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (GamePlayer player in players.Values)
            {
                Positioninfo targetInfo = player.objectinfo.Pos;

                float dx = info.PosX - targetInfo.PosX;
                float dy = info.PosY - targetInfo.PosY;
                float dz = info.PosZ - targetInfo.PosZ;

                float distance = (dx * dx) + (dy * dy) + (dz * dz); // 제곱거리 (루트 연산 없음 → 성능 좋음)

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }

            return closestPlayer.objectId;
        }
    }
}
