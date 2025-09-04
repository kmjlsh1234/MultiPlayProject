using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Game;


namespace Server
{
    public class MatchRoom : Room<ClientSession>
    {
        Dictionary<int, MatchPlayer> players = new Dictionary<int, MatchPlayer>();

        #region :::: Abstract Function
        public override void EnterRoom(ClientSession session)
        {
            MatchPlayer player = new MatchPlayer();
            player.session = session;
            player.playerInfo = new Matchplayerinfo()
            {
                SessionId = session.sessionId,
                IsReady = false
            };
            players.Add(session.sessionId, player);

            //본인에게 정보 전송
            S_Roominfo roomInfoPacket = new S_Roominfo()
            {
                RoomId = roomId,
                MasterId = masterId,
            };

            foreach (MatchPlayer p in players.Values)
            {
                roomInfoPacket.Players.Add(p.playerInfo);
            }
            session.Send(roomInfoPacket);

            //타인에게 정보 전송
            S_Entermatchroom enterRoomPacket = new S_Entermatchroom()
            {
                PlayerInfo = new Matchplayerinfo()
                {
                    SessionId = session.sessionId,
                    NickName = session.nickName,
                    IsReady = false
                }
            };

            BroadCast(enterRoomPacket);
            Console.WriteLine($"Player {session.sessionId} Enter Room {roomId}");
        }

        public override void ExitRoom(ClientSession session)
        {
            players.Remove(session.sessionId);
            session.matchRoom = null;

            if (players.Count == 0)
            {
                RoomManager.Instance.RemoveMatchRoom(roomId);
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
            S_Exitroom leavePacket = new S_Exitroom()
            {
                SessionId = session.sessionId,
            };
            BroadCast(leavePacket);

            Console.WriteLine($"Session {session.sessionId} leave Room");
        }

        public override void BroadCast(IMessage packet)
        {
            foreach (MatchPlayer player in players.Values)
            {
                player.session.Send(packet);
            }
        }

        #endregion

        public void Update()
        {
            Flush();
            Console.WriteLine("MatchRoom Update");
        }

        public int GetPlayerCount()
        {
            return players.Count;
        }

        public void UpdateReadyState(ClientSession session, C_Ready packet)
        {
            MatchPlayer player = null;
            if (players.TryGetValue(session.sessionId, out player))
            {
                player.playerInfo.IsReady = packet.IsReady;

                S_Ready resPacket = new S_Ready()
                {
                    SessionId = session.sessionId,
                    IsReady = packet.IsReady,
                };
                BroadCast(resPacket);
                CheckGameStart();
            }
        }

        public void CheckGameStart()
        {
            int readyCount = 0;
            foreach (MatchPlayer player in players.Values)
            {
                if (player.playerInfo.IsReady)
                {
                    readyCount++;
                }
            }

            if (players.Count == readyCount)
            {
                GameRoom gameRoom = RoomManager.Instance.CreateGameRoom(masterId);
                gameRoom.Init(players.Count);
                foreach(MatchPlayer player in players.Values)
                {
                    gameRoom.EnterRoom(player.session);
                    player.session.gameRoom = gameRoom;
                }     
                
                RoomManager.Instance.StopUpdateMatchRoom(roomId);
            }
        }
    }
}
