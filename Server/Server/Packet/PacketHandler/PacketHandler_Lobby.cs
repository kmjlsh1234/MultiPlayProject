using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using Server.Game;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PacketHandler
{
    public static void C_ConnectHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Connect packet = pkt as C_Connect;

        session.nickName = packet.NickName;
        S_Connect connectPacket = new S_Connect()
        {
            SessionId = session.sessionId,
        };

        session.Send(connectPacket);
    }

    public static void C_CreateroomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        MatchRoom matchRoom = RoomManager.Instance.CreateRoom<MatchRoom>(session.sessionId);
        session.matchRoom = matchRoom;
        matchRoom.Push(matchRoom.EnterRoom, session);
    }

    //TODO : 나중에 Redis API로 변경
    public static void C_CreateorjoinroomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;

        MatchRoom matchRoom = RoomManager.Instance.CreateOrJoinMatchRoom(session);
        session.matchRoom = matchRoom;
        matchRoom.Push(matchRoom.EnterRoom, session);
    }
}
