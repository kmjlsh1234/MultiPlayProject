using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Game;


namespace Server
{
    public class MatchRoom : Room<ClientSession>
    {
        Dictionary<int, Player> players = new Dictionary<int, Player>();

        #region :::: Abstract Function
        public override void EnterRoom(ClientSession session)
        {
            lock (key)
            {
                Player player = PlayerManager.Instance.Add(session);
                players.Add(session.sessionId, player);

                //본인에게 정보 전송
                S_Roominfo roomInfoPacket = new S_Roominfo()
                {
                    RoomId = roomId,
                    MasterId = masterId,
                };

                foreach (Player p in players.Values)
                {
                    PlayerInfo playerInfo = new PlayerInfo()
                    {
                        SessionId = p.session.sessionId,
                        NickName = p.session.nickName,
                        IsReady = p.isReady,
                    };
                    roomInfoPacket.Players.Add(playerInfo);
                }
                session.Send(roomInfoPacket);

                //타인에게 정보 전송
                S_Enterroom enterRoomPacket = new S_Enterroom()
                {
                    PlayerInfo = new PlayerInfo()
                    {
                        SessionId = session.sessionId,
                        NickName = session.nickName,
                        IsReady = false
                    }
                };

                BroadCast(enterRoomPacket);
                Console.WriteLine($"Player {session.sessionId} Enter Room {roomId}");
            }
        }

        public override void ExitRoom(ClientSession session)
        {
            lock (key)
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
            }
            Console.WriteLine($"Session {session.sessionId} leave Room");
        }

        public override void BroadCast(IMessage packet)
        {
            lock (key)
            {
                foreach (Player player in players.Values)
                {
                    player.session.Send(packet);
                }
            }
        }

        #endregion

        public int GetPlayerCount()
        {
            lock (key)
            {
                return players.Count;
            }
        }

        public void UpdateReadyState(ClientSession session, C_Ready packet)
        {
            lock (key)
            {
                Player player = null;
                if (players.TryGetValue(session.sessionId, out player))
                {
                    player.isReady = packet.IsReady;

                    S_Ready resPacket = new S_Ready()
                    {
                        SessionId = session.sessionId,
                        IsReady = packet.IsReady,
                    };
                    BroadCast(resPacket);
                    CheckGameStart();
                }
            }
        }

        public void CheckGameStart()
        {
            int readyCount = 0;
            foreach (Player player in players.Values)
            {
                if (player.isReady)
                {
                    readyCount++;
                }
            }

            if (players.Count == readyCount)
            {
                GameRoom gameRoom = RoomManager.Instance.CreateGameRoom(masterId);
                foreach(Player player in players.Values)
                {
                    GamePlayer gp = new GamePlayer() { session = player.session };
                    gameRoom.EnterRoom(gp);
                    player.session.gameRoom = gameRoom;
                }

                S_Loadingstart packet = new S_Loadingstart();
                BroadCast(packet);
            }
        }
    }
}
