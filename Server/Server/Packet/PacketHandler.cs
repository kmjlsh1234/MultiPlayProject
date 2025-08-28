using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using Server.Game;
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
        
        ClientSession session = s as ClientSession;
        C_Playerinfo packet = pkt as C_Playerinfo;

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
        C_Createroom packet = pkt as C_Createroom;

        GameRoom gameRoom = RoomManager.Instance.CreateRoom(session);

        session.room = gameRoom;
        gameRoom.EnterGame(session);
    }

    public static void C_EnterroomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Enterroom packet = pkt as C_Enterroom;

        //룸 입장 처리
        GameRoom gameRoom = RoomManager.Instance.Find(packet.RoomId);
        if(gameRoom != null)
        {
            session.room = gameRoom;
            gameRoom.EnterGame(session);
        }
        else
        {
            S_Errorcode errorPacket = new S_Errorcode() { Code = ErrorCode.FAIL_ROOM_FIND.Code, Message = ErrorCode.FAIL_ROOM_FIND.Message };
            session.Send(errorPacket);
        }
    }

    public static void C_CreateorjoinroomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;

        GameRoom gameRoom = RoomManager.Instance.CreateOrJoinRoom(session);
        session.room = gameRoom;
        gameRoom.EnterGame(session);
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
        ClientSession session = s as ClientSession;
        session.room.LeaveGame(session);
        //C_ExitRoom packet = pkt as C_ExitRoom;
        //session.room.Push(() => session.room.ExitRoom(session, session.room.roomId));
    }

    

    public static void C_RoomlistHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        Dictionary<int, GameRoom> rooms = RoomManager.Instance.GetRooms();

        S_Roomlist packet = new S_Roomlist();
        foreach(GameRoom room in rooms.Values)
        {
            packet.RoomList.Add(new S_Roominfo()
            {
                RoomId = room.roomId,
                MasterId = room.masterId,
            });
        }
        session.Send(packet);
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
        //session.room.Push(() => session.room.LoadingComplete(session));

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
