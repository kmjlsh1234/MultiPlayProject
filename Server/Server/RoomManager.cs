using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class RoomManager
    {
        public static RoomManager Instance { get; } = new RoomManager();
        public Dictionary<int, Room> roomDic = new Dictionary<int, Room>();

        object key = new object();
        int roomId = 0;
        public void Init()
        {

        }

        public void CreateRoom(ClientSession session, C_CreateRoom packet)
        {
            lock (key)
            {
                Room room = new Room() { roomId = ++this.roomId, roomName = packet.roomName };
                roomDic.Add(roomId, room);
                room.EnterRoom(session);
                Console.WriteLine($"CreateRoom / roomName : {room.roomName}, roomId : {room.roomId}");
            }
        }

        public void RemoveRoom(int roomId)
        {
            lock(key)
            {
                roomDic.Remove(roomId);
                Console.WriteLine("removeRoom");
            }
        }

        public Room FindRoom(int id)
        {
            lock (key)
            {
                Room room = null;
                Console.WriteLine($"FindRoom.roomid : {id}");
                if(roomDic.TryGetValue(id, out room))
                {
                    return room;
                }
                return null;
            }
        }

        public Dictionary<int, Room> GetRoomDic()
        {
            lock (key)
            {
                return roomDic;
            }
        }
    }
}
