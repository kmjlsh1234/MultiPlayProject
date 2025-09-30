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
        Console.WriteLine("Connect Packet Recv");
    }

    public static void C_CreatePartyRoomHandler(Session s, IMessage pkt)
    {

    }

    public static void C_EnterPartyRoomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_EnterPartyRoom packet = pkt as C_EnterPartyRoom;
        //룸 입장 처리
        PartyRoom room = RoomManager.Instance.FindRoom<PartyRoom>(packet.RoomId, RoomType.Party);
        if (room != null)
        {
            //매치 중이거나 게임 중인지 체크
            if (!room.roomState.Equals(RoomState.LobbySate))
            {
                S_ErrorCode errorPacket = ErrorCodeFactory.GetErrorCode(ErrorCode.FAIL_ROOM_FIND);
                session.Send(errorPacket);
                return;
            }
            session.partyRoom = room;
            session.partyRoom.EnterRoom(session);
        }
        else
        {
            S_ErrorCode errorPacket = ErrorCodeFactory.GetErrorCode(ErrorCode.FAIL_ROOM_FIND);
            session.Send(errorPacket);
        }
    }

    public static void C_ExitPartyRoomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        
        session.partyRoom.ExitRoom(session);
    }

    public static void C_CreateRoomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        MatchRoom matchRoom = RoomManager.Instance.CreateRoom<MatchRoom>(session.sessionId);
        session.matchRoom = matchRoom;
        matchRoom.Push(matchRoom.EnterRoom, session);
    }

    //TODO : 나중에 Redis API로 변경
    public static void C_CreateOrJoinRoomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;

        MatchRoom matchRoom = RoomManager.Instance.CreateOrJoinMatchRoom(session, 1);
        session.matchRoom = matchRoom;
        matchRoom.Push(matchRoom.EnterRoom, session);
    }

    public static void C_InviteHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Invite packet = pkt as C_Invite;

        ClientSession targetSession = SessionManager.Instance.FindBySessionId(packet.SessionId);

        //초대 받을 세션 있는지 체크
        if(targetSession == null)
        {
            S_ErrorCode errorPacket = ErrorCodeFactory.GetErrorCode(ErrorCode.SESSION_NOT_FOUND);
            session.Send(errorPacket);
            Console.WriteLine($"ErrorCode : SESSION_NOT_FOUND");
            return;
        }

        //세션이 이미 파티에 가입되어 있는지 체크
        if (targetSession.partyRoom != null)
        {
            S_ErrorCode errorPacket = ErrorCodeFactory.GetErrorCode(ErrorCode.SESSION_ALREADY_IN_ROOM);
            session.Send(errorPacket);
            Console.WriteLine($"ErrorCode : SESSION_ALREADY_IN_ROOM");
            return;
        }

        if(session.partyRoom == null)
        {
            PartyRoom room = RoomManager.Instance.CreateRoom<PartyRoom>(session.sessionId);
            session.partyRoom = room;
            session.partyRoom.EnterRoom(session);
        }

        S_Invite resPacket = new S_Invite()
        {
            RoomId = session.partyRoom.roomId,
            SessionId = session.sessionId,
            NickName = session.nickName
        };

        targetSession.Send(resPacket);
        Console.WriteLine($"Client {session.sessionId} Invite Client {targetSession.sessionId}");
    }

    public static void C_ReadyHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Ready packet = pkt as C_Ready;
        session.partyRoom.UpdateReady(session, packet);
    }
}
