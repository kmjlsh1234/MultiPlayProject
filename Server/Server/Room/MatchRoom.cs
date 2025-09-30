using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Game;
using System.Numerics;


namespace Server
{
    public class MatchRoom : Room
    {
        Dictionary<int, MatchPlayer> players = new Dictionary<int, MatchPlayer>();
        public override int PlayerCount
        {
            get { return players.Count; }
        }
        public MatchRoom()
        {
            roomType = RoomType.Match;
        }

        #region :::: Abstract Function
        public override void EnterRoom(ClientSession session)
        {
            MatchPlayer player = new MatchPlayer();
            player.session = session;
            player.playerInfo = new MatchPlayerInfo()
            {
                SessionId = session.sessionId,
                NickName = session.nickName,
                IsReady = false
            };
            players.Add(session.sessionId, player);

            //본인에게 정보 전송
            S_MatchRoomInfo packet = new S_MatchRoomInfo() { RoomInfo = GenerateRoomInfo() };
            session.Send(packet);

            //타인에게 정보 전송
            S_EnterMatchRoom enterRoomPacket = new S_EnterMatchRoom()
            {
                PlayerInfo = new MatchPlayerInfo()
                {
                    SessionId = session.sessionId,
                    NickName = session.nickName,
                    IsReady = false
                }
            };

            BroadCast(enterRoomPacket);
            Console.WriteLine($"Player {session.sessionId} Enter Room {roomId}");

            CheckGameStart();
        }

        public override void ExitRoom(ClientSession session)
        {
            if(players.TryGetValue(session.sessionId, out MatchPlayer player))
            {
                players.Remove(session.sessionId);
                session.matchRoom = null;

                if (players.Count == 0)
                {
                    RoomManager.Instance.RemoveRoom<MatchRoom>(roomId, roomType);
                }
                else
                {
                    if (masterId == session.sessionId)
                    {
                        int originMasterId = masterId;
                        masterId = players.First().Value.session.sessionId;

                        S_MatchRoomInfo packet = new S_MatchRoomInfo() { RoomInfo = GenerateRoomInfo() };
                        BroadCast(packet);

                        Console.WriteLine($"Master Change {originMasterId} -> {masterId}");
                    }
                }

                //Room에 BroadCast
                S_ExitRoom leavePacket = new S_ExitRoom()
                {
                    SessionId = session.sessionId,
                };
                BroadCast(leavePacket);

                Console.WriteLine($"Session {session.sessionId} leave Room");
            }
        }

        public override void BroadCast(IMessage packet)
        {
            lock (key)
            {
                foreach (MatchPlayer player in players.Values)
                {
                    player.session.Send(packet);
                }
            }
        }

        public override void Update()
        {
            Flush();
        }
        #endregion

        public void CheckGameStart()
        {
            if (PlayerCount.Equals(4))
            {
                MatchFinish();
            }
        }

        public void MatchFinish()
        {
            GameRoom gameRoom = RoomManager.Instance.CreateRoom<GameRoom>(masterId);
            gameRoom.Init(players.Count);
            foreach (MatchPlayer player in players.Values)
            {
                gameRoom.EnterRoom(player.session);
                player.session.gameRoom = gameRoom;
            }

            RoomManager.Instance.StopTickRoom(roomId, roomType);
            Console.WriteLine("GameStart");
        }

        public MatchRoomInfo GenerateRoomInfo()
        {
            MatchRoomInfo roomInfo = new MatchRoomInfo();
            roomInfo.RoomId = roomId;
            roomInfo.MasterId = masterId;
            foreach (MatchPlayer p in players.Values)
            {
                roomInfo.Players.Add(p.playerInfo);
            }

            return roomInfo;
        }
    }
}
