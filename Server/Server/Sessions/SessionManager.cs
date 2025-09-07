using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class SessionManager : SingletonBase<SessionManager>
    {
        int sessionId = 0;
        object key = new object();

        Dictionary<int, ClientSession> sessions = new Dictionary<int, ClientSession>();

        public Session CreateSession()
        {
            lock (key)
            {
                ClientSession session = new ClientSession();
                session.sessionId = ++sessionId;
                sessions.Add(sessionId, session);

                return session;
            }
        }

        public void Remove(ClientSession session)
        {
            lock (key)
            {
                sessions.Remove(session.sessionId);
            }
        }

        public ClientSession FindBySessionId(int sessionId)
        {
            lock (key)
            {
                ClientSession session = null;
                sessions.TryGetValue(sessionId, out session);
                return session;
            }
        }
    }
}
