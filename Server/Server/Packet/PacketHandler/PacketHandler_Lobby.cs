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
    public static void C_PlayerinfoHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Playerinfo packet = pkt as C_Playerinfo;

        session.nickName = packet.NickName;

        S_Connect connectPacket = new S_Connect()
        {
            SessionId = session.sessionId,
        };

        session.Send(connectPacket);
    }

    //TODO : 나중에 Redis API로 변경
    public static void C_CreateroomHandler(Session s, IMessage pkt)
    {

        ClientSession session = s as ClientSession;
        C_Createroom packet = pkt as C_Createroom;

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


    //나중에 Redis API로 변경
    public static void C_RoomlistHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        Dictionary<int, MatchRoom> rooms = RoomManager.Instance.GetMatchRooms();

        S_Roomlist packet = new S_Roomlist();
        foreach (MatchRoom room in rooms.Values)
        {
            packet.RoomList.Add(new S_Roominfo()
            {
                RoomId = room.roomId,
                MasterId = room.masterId,
            });
        }
        session.Send(packet);
    }
}
