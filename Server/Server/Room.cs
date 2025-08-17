using ServerCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    
    public class Room
    {
        //Room Info
        public int roomId;
        public int masterId;
        public string roomName;

        public int readyCount = 0;
        public Dictionary<int, bool> readyDic = new Dictionary<int, bool>();
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
            readyDic.Add(session.sessionId, false);

            //입장 유저에게 리스트 보내기
            S_PlayerList playerListPacket = new S_PlayerList();
            foreach(ClientSession s in sessionList)
            {
                playerListPacket.playerList.Add(
                    new S_PlayerList.Player()
                    {
                        sessionId = s.sessionId,
                        isMaster = (s.sessionId == masterId),
                        isSelf = (s.sessionId == session.sessionId),
                        isReady = readyDic[s.sessionId],
                    });
            }
            
            session.Send(playerListPacket.Write());
            
            //기존 입장한 사람에게 알리기
            S_BroadCast_EnterRoom broadCastPacket = new S_BroadCast_EnterRoom() { sessionId = session.sessionId };
            BroadCast(broadCastPacket.Write());

            Console.WriteLine($"sessionId : {session.sessionId} enter room {session.room.roomName}");
        }

        public void ExitRoom(ClientSession session, int roomId)
        {
            sessionList.Remove(session);

            //나가는 사람이 Master인지 체크
            if (session.sessionId == masterId)
            {
                masterId = sessionList[0].sessionId;
                S_BroadCast_ChangeMaster broadCastPacket = new S_BroadCast_ChangeMaster() { sessionId = masterId };
                BroadCast(broadCastPacket.Write());
                Console.WriteLine($"Master : {session.sessionId} -> {masterId}");
            }
            
            readyDic.Remove(session.sessionId);

            if(sessionList.Count == 0 )
            {
                Program.roomManager.RemoveRoom(roomId);
            }

            //모두에게 알린다
            S_BroadCast_ExitRoom packet = new S_BroadCast_ExitRoom() { sessionId = session.sessionId };
            BroadCast(packet.Write());
        }

        public void Move(ClientSession session, C_MovePacket packet)
        {
            Console.WriteLine($"Client {session.sessionId} :: Pos [{packet.posX},{packet.posY},{packet.posZ}] :: Rotation [{packet.rotY}]");
            //BroadCast
            S_BroadCast_MovePacket broadcastPacket = new S_BroadCast_MovePacket()
            {
                playerId = packet.playerId,
                posX = packet.posX,
                posY = packet.posY,
                posZ = packet.posZ,
                rotY = packet.rotY,
            };
            BroadCast(broadcastPacket.Write());
        }

        public void Ready(ClientSession session, C_ReadyPacket packet)
        {
            readyDic[session.sessionId] = packet.isReady;
            readyCount = packet.isReady ? ++readyCount : --readyCount;

            S_ReadyCheckPacket responsePacket = new S_ReadyCheckPacket() { sessionId = session.sessionId, isReady = packet.isReady };
            session.room.BroadCast(responsePacket.Write());

            if(readyCount == readyDic.Count)
            {

            }
        }
        #endregion
        
    }
}
