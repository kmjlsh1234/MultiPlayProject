using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Room
    {
        
        public Dictionary<int, ClientSession> sessionDic = new Dictionary<int, ClientSession>();
        public object _lock = new object();

        public void EnterRoom(ClientSession session)
        {
            lock( _lock)
            {
                sessionDic.Add(session.sessionId, session);
                session.room = this;
            }    
        }

        public void ExitRoom(ClientSession session)
        {

        }

        public void BroadCast()
        {

        }
    }
}
