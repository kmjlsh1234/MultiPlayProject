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
    public static void C_EnterMatchRoomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_EnterMatchRoom packet = pkt as C_EnterMatchRoom;

        //룸 입장 처리
        MatchRoom room = RoomManager.Instance.FindRoom<MatchRoom>(packet.RoomId, RoomType.Match);
        if (room != null)
        {
            session.matchRoom = room;
            room.Push(room.EnterRoom, session);
        }
        else
        {
            S_ErrorCode errorPacket = ErrorCodeFactory.GetErrorCode(ErrorCode.FAIL_ROOM_FIND);
            session.Send(errorPacket);
        }
    }

    public static void C_ExitRoomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        session.matchRoom.Push(session.matchRoom.ExitRoom, session);
    }

    public static void C_StartHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        session.matchRoom.Push(session.matchRoom.MatchFinish);
    }
}
