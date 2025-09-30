using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Game;
using ServerCore;

namespace Server
{
    public class PartyRoom : Room
    {
        Dictionary<int, PartyPlayer> players = new Dictionary<int, PartyPlayer>();
        
        public PartyRoom() 
        {
            this.roomState = RoomState.LobbySate;
            this.roomType = RoomType.Party;
            this.maxCount = 4;
        }

        #region :::: Abstract Function
        public override void BroadCast(IMessage packet)
        {
            foreach(PartyPlayer player in players.Values)
            {
                player.session.Send(packet);
            }
        }

        public override void EnterRoom(ClientSession session)
        {
            //방 인원 체크
            if (players.Count.Equals(maxCount))
            {
                S_ErrorCode errrorPacket = ErrorCodeFactory.GetErrorCode(ErrorCode.MAX_ROOM_COUNT);
                session.Send(errrorPacket);
                return;
            }
            PartyPlayerInfo playerinfo = new PartyPlayerInfo()
            {
                SessionId = session.sessionId,
                NickName = session.nickName,
                IsReady = false
            };

            PartyPlayer player = new PartyPlayer()
            {
                session = session,
                playerInfo = playerinfo
            };

            players.Add(session.sessionId, player);

            //본인에게 정보 전송
            S_PartyRoomInfo packet = new S_PartyRoomInfo() { RoomInfo = GetPartyRoomInfo() };
            session.Send(packet);

            //타인에게 정보 전송
            S_EnterPartyRoom enterRoomPacket = new S_EnterPartyRoom()
            {
                PlayerInfo = playerinfo
            };

            BroadCast(enterRoomPacket);
            Console.WriteLine($"Player {session.sessionId} Enter Party Room {roomId}");
        }

        public override void ExitRoom(ClientSession session)
        {
            if (players.TryGetValue(session.sessionId, out PartyPlayer player))
            {
                players.Remove(session.sessionId);
                session.partyRoom = null;

                if (players.Count == 0)
                {
                    RoomManager.Instance.RemoveRoom<PartyRoom>(roomId, roomType);
                }
                else
                {
                    if (masterId.Equals(session.sessionId))
                    {
                        int originMasterId = masterId;
                        masterId = players.First().Value.session.sessionId;

                        S_PartyRoomInfo packet = new S_PartyRoomInfo() { RoomInfo = GetPartyRoomInfo() };
                        BroadCast(packet);

                        Console.WriteLine($"Party Master Change {originMasterId} -> {masterId}");
                    }
                }

                //Room에 BroadCast
                S_ExitPartyRoom leavePacket = new S_ExitPartyRoom()
                {
                    SessionId = session.sessionId,
                };
                BroadCast(leavePacket);

                Console.WriteLine($"Session {session.sessionId} leave Party Room");

                //Ready 상태 초기화
                foreach(PartyPlayer p in players.Values)
                {
                    player.playerInfo.IsReady = false;
                }
            }
        }

        public override void Update()
        {
        }
        #endregion

        public PartyRoomInfo GetPartyRoomInfo()
        {
            PartyRoomInfo roomInfo = new PartyRoomInfo();
            roomInfo.RoomId = roomId;
            roomInfo.MasterId = masterId;
            foreach (PartyPlayer player in players.Values)
            {
                roomInfo.Players.Add(player.playerInfo);
            }

            return roomInfo;
        }

        public void UpdateReady(ClientSession session, C_Ready packet)
        {
            if (players.TryGetValue(session.sessionId, out PartyPlayer player))
            {
                player.playerInfo.IsReady = packet.IsReady;

                S_Ready resPacket = new S_Ready()
                {
                    SessionId = session.sessionId,
                    IsReady = packet.IsReady,
                };
                BroadCast(resPacket);


                if (CheckMatchStart())
                {
                    players.TryGetValue(masterId, out PartyPlayer master);
                    MatchRoom room = RoomManager.Instance.CreateOrJoinMatchRoom(master.session, players.Count);
                    foreach(PartyPlayer p in players.Values)
                    {
                        p.session.partyRoom.roomState = RoomState.MatchState;

                        p.session.matchRoom = room;
                        room.Push(room.EnterRoom, p.session);
                    }
                }
            }
        }

        private bool CheckMatchStart()
        {
            int readyCount = 0;
            foreach(PartyPlayer player in players.Values)
            {
                if (player.playerInfo.IsReady)
                {
                    readyCount++;
                }
            }

            return readyCount.Equals(players.Values.Count);
        }
    }
}
