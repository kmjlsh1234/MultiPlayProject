using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class RoomManager
    {
        public List<Room> roomList = new List<Room>();
        object _lock = new object();

        public void Init()
        {

        }

        public void CreateRoom(ClientSession session)
        {
            lock (_lock)
            {
                Room room = new Room();
                roomList.Add(room);

                room.EnterRoom(session);
            }
        }

        public List<Room> GetRoomList(ClientSession session)
        {
            lock (_lock)
            {
                return roomList;
            }
        }
    }
}
