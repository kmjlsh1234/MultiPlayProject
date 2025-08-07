using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Room
    {
        
        public List<ClientSession> sessionList = new List<ClientSession>();
        public object _lock = new object();

        public void EnterRoom(ClientSession session)
        {
            lock( _lock)
            {
                sessionList.Add(session);
                session.room = this;
            }    
        }

        public List<S_PlayerList.Player> getPlayerList()
        {
            lock (_lock)
            {
                List<S_PlayerList.Player> list = new List<S_PlayerList.Player>();
                foreach ( ClientSession session in sessionList)
                {
                    S_PlayerList.Player player = new S_PlayerList.Player();
                    player.sessionId = session.sessionId;
                    list.Add(player);
                }
                return list; 
            }
        }

        public void ExitRoom(ClientSession session)
        {

        }

        public void BroadCast(ArraySegment<byte> buffer)
        {
            foreach(ClientSession session in sessionList)
            {
                session.Send(buffer);
            }
        }
    }
}
