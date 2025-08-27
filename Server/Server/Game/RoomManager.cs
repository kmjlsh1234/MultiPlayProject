using Google.Protobuf.Protocol;
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

        Dictionary<int, GameRoom> rooms = new Dictionary<int, GameRoom>();
        int roomId = 1;

        public GameRoom CreateRoom(ClientSession session)
        {
            GameRoom gameRoom = new GameRoom();

            lock (key)
            {
                gameRoom.roomId = roomId;
                gameRoom.masterId = session.sessionId;
                rooms.Add(roomId, gameRoom);
                roomId++;
                Console.WriteLine($"CreateRoom / roomId : {gameRoom.roomId}");
            }

            return gameRoom;
        }

        public GameRoom CreateOrJoinRoom(ClientSession session)
        {
            lock (key)
            {
                foreach(GameRoom room in rooms.Values)
                {
                    if(room.isPublic && room.playerCount < 4)
                    {
                        return room;
                    }
                }
            }

            GameRoom gameRoom = CreateRoom(session);
            return gameRoom;
        }

        public bool RemoveRoom(int roomId)
        {
            lock (key)
            {
                Console.WriteLine($"Room {roomId} Removed");
                return rooms.Remove(roomId);
            }
        }

        public GameRoom Find(int roomId)
        {
            lock(key)
            {
                GameRoom gameRoom = null;
                if(rooms.TryGetValue(roomId, out gameRoom))
                {
                    return gameRoom;
                }
                return null;
            }
        }

        public Dictionary<int, GameRoom> GetRooms()
        {
            lock( key)
            {
                return rooms;
            }
        }
    }
}
