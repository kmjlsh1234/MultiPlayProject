using Google.Protobuf;
using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class PacketHandler
{
    public static void C_PingHandler(Session s, IMessage pkt)
    {
        //s.Send(new S_PongPacket().Write());
    }

    public static void C_PlayerinfoHandler(Session s, IMessage pkt)
    {
        /*
        ClientSession session = s as ClientSession;
        C_PlayerInfoPacket packet = pkt as C_PlayerInfoPacket;

        session.nickName = packet.nickName;
        
        S_MoveLobbyPacket resPacket = new S_MoveLobbyPacket() { sessionId = session.sessionId };
        session.Send(resPacket.Write());
        Console.WriteLine($"Session NickName : {packet.nickName}");
        */
    }

    public static void C_CreateroomHandler(Session s, IMessage pkt)
    {
        /*
        ClientSession session = s as ClientSession;
        C_CreateRoom packet = pkt as C_CreateRoom;

        Program.roomManager.CreateRoom(session, packet);
        */
    }

    public static void C_EnterroomHandler(Session s, IMessage pkt)
    {
        /*
        ClientSession session = s as ClientSession;
        C_EnterRoom packet = pkt as C_EnterRoom;

        //룸 입장 처리
        session.room = Program.roomManager.FindRoomById(packet.roomId);
        
        if (session.room != null)
        {
            session.room.Push(() => session.room.EnterRoom(session));
        }
        else
        {
            S_ErrorCode errorPacket = new S_ErrorCode() { code = ErrorCode.FAIL_ROOM_FIND.Code, message = ErrorCode.FAIL_ROOM_FIND.Message };
            session.Send(errorPacket.Write());
        }
        */
    }

    public static void C_CreateorjoinroomHandler(Session s, IMessage pkt)
    {
        /*
        ClientSession session = s as ClientSession;

        Room room = Program.roomManager.FindAvailableRoom();
        if(room != null)
        {
            session.room = room;
            session.room.Push(() => session.room.EnterRoom(session));
        }
        else
        {
            C_CreateRoom packet = new C_CreateRoom() { roomName = "Random Room"};
            Program.roomManager.CreateRoom(session, packet);
        }
        */
    }
    
    public static void C_ChatHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        /*
        C_Chat packet = pkt as C_Chat;
        S_BroadCast_Chat broadCastPacket = new S_BroadCast_Chat() 
        { 
            sessionId = session.sessionId,
            message = packet.message
        };
        Console.WriteLine($"C_ChatHandler");
        Console.WriteLine($"message : {packet.message}");
        session.room.BroadCast(broadCastPacket.Write());
        */
    }
    
    

    public static void C_ExitroomHandler(Session s, IMessage pkt)
    {
        //ClientSession session = s as ClientSession;
        //C_ExitRoom packet = pkt as C_ExitRoom;

        //session.room.Push(() => session.room.ExitRoom(session, session.room.roomId));
    }

    

    public static void C_RoomlistHandler(Session s, IMessage pkt)
    {
        //ClientSession session = s as ClientSession;
        //S_RoomList packet = new S_RoomList();

        //Dictionary<int, Room> dic = Program.roomManager.GetRoomDic();
        //foreach (KeyValuePair<int, Room> pair in dic)
        //{
        //    packet.roomList.Add(new S_RoomList.Room() 
        //    { 
        //        roomId = pair.Value.roomId, 
        //        roomName = pair.Value.roomName,
        //        playerCount = pair.Value.sessionList.Count,
        //    });
        //}

        //session.Send(packet.Write());
    }

    public static void C_MoveHandler(Session s, IMessage pkt)
    {
        //ClientSession session = s as ClientSession;
        //C_MovePacket packet = pkt as C_MovePacket;
        
        //session.room.Push(() => session.room.Move(session, packet));
    }

    public static void C_InputHandler(Session s, IMessage pkt)
    {
        //ClientSession session = s as ClientSession;
        //C_InputPacket packet = pkt as C_InputPacket;

        //session.room.Push(() => session.room.PlayerMove(session, packet));
    }

    public static void C_ReadyHandler(Session s, IMessage pkt)
    {
        //ClientSession session = s as ClientSession;
        //C_ReadyPacket packet = pkt as C_ReadyPacket;

        //session.room.Push(() => session.room.Ready(session, packet));

    }

    public static void C_StartHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;

    }

    public static void C_LoadingcompleteHandler(Session s, IMessage pkt)
    {        
        ClientSession session = s as ClientSession;
        session.room.Push(() => session.room.LoadingComplete(session));

    }

    public static void C_InviteHandler(Session s, IMessage pkt)
    {
        //ClientSession session = s as ClientSession;
        //C_InvitePacket packet = pkt as C_InvitePacket;

        //ClientSession targetSession = SessionManager.Instance.FindBySessionId(packet.sessionId);
        //if (targetSession != null)
        //{
        //    S_InvitePacket resPacket = new S_InvitePacket()
        //    {
        //        roomId = session.room.roomId,
        //        sendUserNickName = session.nickName
        //    };

        //    targetSession.Send(resPacket.Write());
        //    Console.WriteLine($"Client {session.sessionId} Invite Client {targetSession.sessionId}");
        //}
        //else
        //{
        //    Console.WriteLine("targetSession is Null");
        //}
    }
}
