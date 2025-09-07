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
        MatchRoom room = RoomManager.Instance.FindMatchRoom(packet.RoomId);
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

    public static void C_ReadyHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Ready packet = pkt as C_Ready;
        session.matchRoom.UpdateReadyState(session, packet);
    }

    public static void C_InviteHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Invite packet = pkt as C_Invite;

        ClientSession targetSession = SessionManager.Instance.FindBySessionId(packet.SessionId);
        if (targetSession != null)
        {
            if(targetSession.matchRoom != null)
            {
                S_Errorcode errorPacket = ErrorCodeFactory.GetErrorCode(ErrorCode.SESSION_ALREADY_IN_ROOM);
                session.Send(errorPacket);
                return;
            }

            S_Invite resPacket = new S_Invite()
            {
                RoomId = session.matchRoom.roomId,
                SessionId = session.sessionId,
                NickName = session.nickName
            };

            targetSession.Send(resPacket);
            Console.WriteLine($"Client {session.sessionId} Invite Client {targetSession.sessionId}");
        }
        else
        {
            S_Errorcode errorPacket = ErrorCodeFactory.GetErrorCode(ErrorCode.SESSION_NOT_FOUND);
            session.Send(errorPacket);
        }
    }
}
