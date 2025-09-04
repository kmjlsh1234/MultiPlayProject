using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class RoomManager : SingletonBase<RoomManager>
    {

        object key = new object();

        Dictionary<int, System.Timers.Timer> gameTimers = new Dictionary<int, System.Timers.Timer>();
        Dictionary<int, System.Timers.Timer> matchTimers = new Dictionary<int, System.Timers.Timer>();

        //Room Collections
        Dictionary<int, MatchRoom> matchRooms = new Dictionary<int, MatchRoom>();
        Dictionary<int, GameRoom> gameRooms = new Dictionary<int, GameRoom>();

        int matchRoomId = 1;
        int gameRoomId = 1;

        public void StartTickRoom<R>(R room, int tick = 100) where R : Room
        {
            var timer = new System.Timers.Timer();
            timer.Interval = tick;
            timer.Elapsed += ((s, e) => { room.Update(); });
            timer.AutoReset = true;
            timer.Enabled = true;

            lock (key)
            {
                switch (room.roomType)
                {
                    case RoomType.Match:
                        matchTimers.Add(room.roomId, timer);
                        break;
                    case RoomType.Game:
                        gameTimers.Add(room.roomId, timer);
                        break;
                    default:
                        return;
                }
            }
        }

        public void StopTickRoom(int roomId, RoomType roomType) 
        {
            lock (key)
            {
                System.Timers.Timer timer = null;
                switch (roomType)
                {
                    case RoomType.Match:
                        matchTimers.TryGetValue(roomId, out timer);
                        if(timer != null)
                        {
                            timer.Enabled = false;
                            matchTimers.Remove(roomId);
                        }
                        break;
                    case RoomType.Game:
                        gameTimers.TryGetValue(roomId, out timer);
                        if (timer != null)
                        {
                            timer.Enabled = false;
                            gameTimers.Remove(roomId);
                        }
                        break;
                    default:
                        return;
                }
            }
        }

        #region ::::MatchRoom
        public MatchRoom CreateMatchRoom(ClientSession session)
        {
            MatchRoom room = new MatchRoom();

            lock (key)
            {
                room.roomId = matchRoomId;
                room.masterId = session.sessionId;
                matchRooms.Add(matchRoomId, room);
                matchRoomId++;
                StartTickRoom(room, 500);
                Console.WriteLine($"Create Match Room / roomId : {room.roomId}");
            }

            return room;
        }

        public MatchRoom CreateOrJoinMatchRoom(ClientSession session)
        {
            lock (key)
            {
                foreach(MatchRoom room in matchRooms.Values)
                {
                    if(room != null)
                    {
                        return room;
                    }
                }
            }

            MatchRoom matchRoom = CreateMatchRoom(session);
            return matchRoom;
        }

        public bool RemoveMatchRoom(int roomId)
        {
            lock (key)
            {
                Console.WriteLine($"Match Room {roomId} Removed");
                StopTickRoom(roomId, RoomType.Match);
                return matchRooms.Remove(roomId);
            }
        }

        public MatchRoom FindMatchRoom(int roomId)
        {
            lock(key)
            {
                MatchRoom room = null;
                if(matchRooms.TryGetValue(roomId, out room))
                {
                    return room;
                }
                return null;
            }
        }

        public Dictionary<int, MatchRoom> GetMatchRooms()
        {
            lock( key)
            {
                return matchRooms;
            }
        }
        #endregion

        #region ::::GameRoom
        public GameRoom CreateGameRoom(int masterId)
        {
            GameRoom room = new GameRoom();

            lock (key)
            {
                room.roomId = gameRoomId;
                room.masterId = masterId;
                gameRooms.Add(gameRoomId, room);
                gameRoomId++;
                Console.WriteLine($"Create Game Room / roomId : {room.roomId}");
                StartTickRoom(room, 50);
            }
            return room;
        }

        public bool RemoveGameRoom(int roomId)
        {
            lock (key)
            {
                Console.WriteLine($"Game Room {roomId} Removed");
                StopTickRoom(roomId, RoomType.Game);
                return gameRooms.Remove(roomId);
            }
        }

        public Dictionary<int, GameRoom> GetGameRooms()
        {
            lock (key)
            {
                return gameRooms;
            }
        }
        #endregion
    }
}
