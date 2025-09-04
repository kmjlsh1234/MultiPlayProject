using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class RoomManager
    {
        public static RoomManager Instance { get; } = new RoomManager();

        object key = new object();
        Dictionary<int, System.Timers.Timer> gameRoomTimers = new Dictionary<int, System.Timers.Timer>();
        Dictionary<int, System.Timers.Timer> matchRoomTimers = new Dictionary<int, System.Timers.Timer>();
        Dictionary<int, MatchRoom> matchRooms = new Dictionary<int, MatchRoom>();
        Dictionary<int, GameRoom> gameRooms = new Dictionary<int, GameRoom>();

        int matchRoomId = 1;
        int gameRoomId = 1;

        public void StartUpdateGameRoom(GameRoom room, int tick = 100)
        {
            var timer = new System.Timers.Timer();
            timer.Interval = tick;
            timer.Elapsed += ((s, e) => { room.Update(); });
            timer.AutoReset = true;
            timer.Enabled = true;

            lock (key)
            {
                gameRoomTimers.Add(room.roomId, timer);
            }
            
        }

        public void StopUpdateMatchRoom(int roomId)
        {
            lock (key)
            {
                System.Timers.Timer timer = null;
                matchRoomTimers.TryGetValue(roomId, out timer);
                if (timer != null)
                {
                    timer.Enabled = false;
                    matchRoomTimers.Remove(roomId);
                }
            }
        }

        public void StartUpdateMatchRoom(MatchRoom room, int tick = 100)
        {
            var timer = new System.Timers.Timer();
            timer.Interval = tick;
            timer.Elapsed += ((s, e) => { room.Update(); });
            timer.AutoReset = true;
            timer.Enabled = true;
            lock (key)
            {
                matchRoomTimers.Add(room.roomId, timer);
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
                StartUpdateMatchRoom(room, 500);
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
                    if(room.GetPlayerCount() < 4)
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
                System.Timers.Timer timer = null;
                matchRoomTimers.TryGetValue(roomId, out timer);
                if (timer != null)
                {
                    timer.Enabled = false;
                    matchRoomTimers.Remove(roomId);
                }
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
                StartUpdateGameRoom(room, 50);
            }
            return room;
        }

        public bool RemoveGameRoom(int roomId)
        {
            lock (key)
            {
                Console.WriteLine($"Game Room {roomId} Removed");
                System.Timers.Timer timer = null;
                gameRoomTimers.TryGetValue(roomId, out timer);
                if (timer != null)
                {
                    timer.Enabled = false;
                    gameRoomTimers.Remove(roomId);
                }
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
