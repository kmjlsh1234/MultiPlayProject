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
    public static void C_EntermatchroomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Entermatchroom packet = pkt as C_Entermatchroom;

        //룸 입장 처리
        MatchRoom room = RoomManager.Instance.FindRoom<MatchRoom>(packet.RoomId, RoomType.Match);
        if (room != null)
        {
            session.matchRoom = room;
            room.Push(room.EnterRoom, session);
        }
        else
        {
            S_Errorcode errorPacket = ErrorCodeFactory.GetErrorCode(ErrorCode.FAIL_ROOM_FIND);
            session.Send(errorPacket);
        }
    }

    public static void C_ExitroomHandler(Session s, IMessage pkt)
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
