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
        private static Random rand = new Random();

        public void Init()
        {

        }

        public void CreateRoom(ClientSession session, C_CreateRoom packet)
        {
            lock (key)
            {
                ++this.roomId;
                Room room = new Room() { roomId = this.roomId, roomName = packet.roomName };
                room.masterId = session.sessionId;
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

        public Room FindRoomById(int id)
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

        public Room FindAvailableRoom()
        {
            lock (key)
            {
                Room room = null;

                if(roomDic.Count > 0)
                {
                    var keys = new List<int>(roomDic.Keys);

                    // 난수 인덱스 뽑기
                    int randomIndex = rand.Next(keys.Count);

                    // 랜덤 키에 해당하는 Room 리턴
                    return roomDic[keys[randomIndex]];
                }
                else
                {
                    return null;
                }
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
