using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class RoomManager
    {
        public static RoomManager Instance { get; } = new RoomManager();
        public List<Room> roomList = new List<Room>();
        object key = new object();

        public void Init()
        {

        }

        public void CreateRoom(ClientSession session)
        {
            lock (key)
            {
                Room room = new Room();
                roomList.Add(room);

                room.EnterRoom(session);
            }
        }

        public List<Room> GetRoomList(ClientSession session)
        {
            lock (key)
            {
                return roomList;
            }
        }
    }
}
