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

        int roomId = 0;

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

        public T CreateRoom<T>(int masterId) where T : Room, new()
        {
            T room = new T();

            lock (key)
            {
                room.roomId = roomId;
                room.masterId = masterId;
                switch (room.roomType)
                {
                    case RoomType.Match:
                        matchRooms.Add(roomId, room as MatchRoom);
                        StartTickRoom(room, 500);
                        break;
                    case RoomType.Game:
                        gameRooms.Add(roomId, room as GameRoom);
                        StartTickRoom(room, 50);
                        break;
                    default:
                        return null;
                }
                
            }
            Console.WriteLine($"Create {room.roomType.ToString()} Room / roomId : {room.roomId}");
            roomId++;
            return room;
        }

        public bool RemoveRoom<T>(int roomId, RoomType roomType) where T : Room, new()
        {
            lock (key)
            {
                Console.WriteLine($"{roomType.ToString()} Room {roomId} Removed");
                StopTickRoom(roomId, roomType);
                switch (roomType)
                {
                    case RoomType.Match:
                        return matchRooms.Remove(roomId);
                    case RoomType.Game:
                        return gameRooms.Remove(roomId);
                    default:
                        return false;
                }
            }
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

            MatchRoom matchRoom = CreateRoom<MatchRoom>(session.sessionId);
            return matchRoom;
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

        
    }
}
