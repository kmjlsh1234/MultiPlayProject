using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Game;
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
        public int PlayerCount { get { return players.Count; } }
        public Dictionary<int, GamePlayer> players = new Dictionary<int, GamePlayer>();
        public Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();

        public Map map { get; set; } = new Map(1.5f, -30, -30, 30, 30);
        public SpawningPool spawningPool { get; set; }
        public int count = 0;
        public int loadingCompleteCount = 0;
        public int randomSeed { get; set; }

        public int level = 1;
        public int exp = 0;
        public int maxExp = 10;

        bool isSkillSelect = false;

        public GameRoom()
        {
            roomType = RoomType.Game;
            randomSeed = Environment.TickCount;
            spawningPool = new SpawningPool(this);
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
            GamePlayer player = new GamePlayer(session, this);
            players.Add(player.session.sessionId, player);

            //나에게 정보 전송
            Console.WriteLine($"Session {session.sessionId} Enter GameRoom");
            
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
            foreach (GamePlayer player in players.Values)
            {
                player.Update();
            }

            foreach (Enemy enemy in enemies.Values)
            {
                enemy.Update();
            }

            Flush();
        }
        #endregion

        public void CheckGameStart(ClientSession session)
        {
            loadingCompleteCount++;
            if (loadingCompleteCount.Equals(PlayerCount))
            {
                S_Gameroominfo packet = new S_Gameroominfo();
                
                Gameroominfo info = new Gameroominfo();
                info.RoomId = roomId;
                info.MasterId = masterId;
                info.RandomSeed = randomSeed;

                foreach(GamePlayer gamePlayer in players.Values)
                {
                    info.Players.Add(gamePlayer.objectinfo);
                }

                packet.RoomInfo = info;

                BroadCast(packet);
                PushAfter(1500, SpawnEnemy);
            }
        }

        public bool Init(int count)
        {
            this.count = count;
            //map.LoadMap("MapData");
            
            return true;
        }

        #region :::: Player Control
        public void HandleMove(ClientSession session, C_Move packet)
        {
            if(players.TryGetValue(session.sessionId, out GamePlayer player))
            {
                player.objectinfo.Pos.PosX = packet.PosX;
                player.objectinfo.Pos.PosY = packet.PosY;
                player.objectinfo.Pos.PosZ = packet.PosZ;
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

        public void HandleInput(ClientSession session, C_Input packet)
        {
            if(players.TryGetValue(session.sessionId, out GamePlayer player))
            {
                //TODO : 유저 위치 업데이트
                //UpdatePlayerPosition(player, packet);

                player.objectinfo.CellInfo = packet.CellInfo;
                
                S_Input movePacket = new S_Input()
                {
                    SessionId = player.session.sessionId,
                    Input = new Inputinfo()
                    {
                        X = packet.Input.X,
                        Y = packet.Input.Y,
                        Z = packet.Input.Z,
                    },
                    CellInfo = packet.CellInfo,
                };

                BroadCast(movePacket);
                //Console.WriteLine($"{player.objectId} : [ {packet.CellInfo.X}, {packet.CellInfo.Y}]");
                //Console.WriteLine($"{player.objectId} : [ {movePacket.DirX}, {movePacket.DirY}, {movePacket.DirZ}]");
            }
        }

        #endregion
        public void HandleEnemyMove(ClientSession session, C_Enemymove packet)
        {
            S_Enemymove resPacket = new S_Enemymove();
            Console.WriteLine($"EnemyArea Packet Size : {packet.CalculateSize()}");
            Console.WriteLine($"EnemyCount : {packet.Enemies.Count}");
            foreach(Objectinfo info in packet.Enemies)
            {
                if(enemies.TryGetValue(info.ObjectId, out Enemy enemy))
                {
                    enemy.objectinfo.Pos = info.Pos;
                    resPacket.Enemies.Add(info);
                }
            }
            BroadCast(resPacket);
        }

        public void SpawnEnemy()
        {
            if (isSkillSelect) return;

            Enemy enemy = spawningPool.TrySpawn();
            enemies.Add(enemy.objectId, enemy);

            S_Spawnenemy packet = new S_Spawnenemy()
            {
                ObjectInfo = enemy.objectinfo
            };

            BroadCast(packet);
            PushAfter(1500, SpawnEnemy);
        }

        public void AddExp(ClientSession session, C_Exp packet)
        {
            exp = packet.ExpCount;

            S_Exp resPacket = new S_Exp() {  ExpCount = exp };
            BroadCast(resPacket);

            if (exp >= maxExp)
            {
                LevelUp();
            }
        }

        #region :::: LevelUp & SkillSelect
        void LevelUp()
        {
            isSkillSelect = true;
            level++;
            maxExp *= 2;
            exp = 0;

            S_Levelup packet = new S_Levelup();
            BroadCast(packet);

            PushAfter(10000, LevelUpFinish);
            //TODO : 스킬 선택 타이머 시작
        }

        
        public void SkillSelect(ClientSession session, IMessage pkt)
        {
            if(players.TryGetValue(session.sessionId, out GamePlayer player))
            {
                player.isSkillSelect = true;
            }

            switch (pkt.GetType() as IMessage)
            {
                case C_Upgradeskill:
                    C_Upgradeskill upgradeSkill = pkt as C_Upgradeskill;
                    player.skillManageComponent.UpgradeSkill(upgradeSkill.Skillinfo.Id);
                    break;
                case C_Newskill:
                    C_Newskill newSkill = pkt as C_Newskill;
                    player.skillManageComponent.AddSkill(newSkill.Skillinfo);
                    break;
                default:
                    break;
            }

            int count = 0;
            foreach(GamePlayer p in players.Values)
            {
                if (p.isSkillSelect)
                {
                    count++;
                }
            }

            S_Skillselect selectPacket = new S_Skillselect();
            BroadCast(selectPacket);

            if (count.Equals(PlayerCount))
            {
                LevelUpFinish();
            }

        }

        private void LevelUpFinish()
        {
            if (!isSkillSelect) return;

            isSkillSelect = false;
            BroadCast(new S_Levelupfinish());
            PushAfter(1500, SpawnEnemy);
        }
        #endregion
    }
}
