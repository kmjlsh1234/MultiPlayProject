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
        Dictionary<int, PartyRoom> partyRooms = new Dictionary<int, PartyRoom>();
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
                    case RoomType.Party:
                        PartyRoom partyRoom = room as PartyRoom;
                        partyRooms.Add(roomId, partyRoom);
                        break;

                    case RoomType.Match:
                        MatchRoom matchRoom = room as MatchRoom;
                        matchRooms.Add(roomId, matchRoom);
                        StartTickRoom(room, 500);

                        break;
                    case RoomType.Game:
                        GameRoom gameRoom = room as GameRoom;
                        gameRooms.Add(roomId, gameRoom);
                        StartTickRoom(room, 50);
                        break;
                    default:
                        return null;
                }
                
                roomId++;
            }
            Console.WriteLine($"Create {room.roomType.ToString()} Room [{room.roomId}]");
            
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
                    case RoomType .Party:
                        return partyRooms.Remove(roomId);
                    case RoomType.Match:
                        return matchRooms.Remove(roomId);
                    case RoomType.Game:
                        return gameRooms.Remove(roomId);
                    default:
                        return false;
                }
            }
        } 

        public MatchRoom CreateOrJoinMatchRoom(ClientSession session, int playerCount)
        {
            lock (key)
            {
                foreach(MatchRoom room in matchRooms.Values)
                {
                    if(room.PlayerCount + playerCount <= 4)
                    {
                        
                        return room;
                    }
                }
            }

            MatchRoom matchRoom = CreateRoom<MatchRoom>(session.sessionId);
            return matchRoom;
        }
        public T FindRoom<T>(int roomId, RoomType type) where T : Room
        {
            lock (key)
            {
                switch (type)
                {
                    case RoomType.Match:
                        if (matchRooms.TryGetValue(roomId, out var matchRoom))
                            return matchRoom as T;
                        break;

                    case RoomType.Party:
                        if (partyRooms.TryGetValue(roomId, out var partyRoom))
                            return partyRoom as T;
                        break;
                    case RoomType.Game:
                        if (gameRooms.TryGetValue(roomId, out var gameRoom))
                            return gameRoom as T;
                        break;
                    default:
                        return null;
                }
                return null;
            }
        }
    }
}
