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
        public List<ArraySegment<byte>> pendingList = new List<ArraySegment<byte>>();
        public JobQueue JobQueue = new JobQueue();

        public object _lock = new object();

        public void Push(Action job)
        {
            JobQueue.Push(job);
        }

        public void Flush()
        {
            foreach (ClientSession session in sessionList)
            {
                session.Send(pendingList);
            }
            pendingList.Clear();
        }

        public void BroadCast(ArraySegment<byte> buffer)
        {
            pendingList.Add(buffer);
        }

        #region Room Function

        public void EnterRoom(ClientSession session)
        {
            
            session.room = this;
            sessionList.Add(session);

            //입장 유저에게 리스트 보내기
            S_PlayerList playerListPacket = new S_PlayerList();
            foreach(ClientSession s in sessionList)
            {
                playerListPacket.playerList.Add(
                    new S_PlayerList.Player()
                    {
                        sessionId = s.sessionId,
                        isSelf = (s.sessionId == session.sessionId),
                    });
            }
            
            session.Send(playerListPacket.Write());
            
            //기존 입장한 사람에게 알리기
            S_BroadCast_EnterRoom broadCastPacket = new S_BroadCast_EnterRoom() { sessionId = session.sessionId };
            BroadCast(broadCastPacket.Write());
        }

        public void ExitRoom(ClientSession session)
        {
            sessionList.Remove(session);

            //모두에게 알린다
            S_BroadCast_ExitRoom packet = new S_BroadCast_ExitRoom() { sessionId = session.sessionId };
            BroadCast(packet.Write());
        }
        #endregion
        
    }
}
