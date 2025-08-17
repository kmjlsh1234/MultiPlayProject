using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PacketHandler
{
    
    public static void C_ReadyPacketHandler(Session s, IPacket pkt)
    {
        ClientSession session = s as ClientSession;
        C_ReadyPacket packet = pkt as C_ReadyPacket;

        session.room.Push(() => session.room.Ready(session, packet));
        
    }

    public static void C_ChatHandler(Session s, IPacket pkt)
    {
        ClientSession session = s as ClientSession;

        C_Chat packet = pkt as C_Chat;
        S_BroadCast_Chat broadCastPacket = new S_BroadCast_Chat() 
        { 
            sessionId = session.sessionId,
            message = packet.message
        };
        Console.WriteLine($"C_ChatHandler");
        Console.WriteLine($"message : {packet.message}");
        session.room.BroadCast(broadCastPacket.Write());
    }
    
    public static void C_EnterRoomHandler(Session s, IPacket pkt)
    {
        //패킷 파싱
        ClientSession session = s as ClientSession;
        C_EnterRoom packet = pkt as C_EnterRoom;
        

        //룸 입장 처리
        session.room = Program.roomManager.FindRoom(packet.roomId);
        if(session.room != null)
        {
            session.room.Push(() => session.room.EnterRoom(session));
        }
        else
        {
            S_ErrorCode errorPacket = new S_ErrorCode() { code = ErrorCode.FAIL_ROOM_FIND.Code, message = ErrorCode.FAIL_ROOM_FIND.Message };
            session.Send(errorPacket.Write());
        }
        
    }

    public static void C_ExitRoomHandler(Session s, IPacket pkt)
    {
        ClientSession session = s as ClientSession;
        C_ExitRoom packet = pkt as C_ExitRoom;

        session.room.Push(() => session.room.ExitRoom(session, session.room.roomId));
    }

    public static void C_CreateRoomHandler(Session s, IPacket pkt)
    {
        ClientSession session = s as ClientSession;
        C_CreateRoom packet = pkt as C_CreateRoom;

        Program.roomManager.CreateRoom(session, packet);
    }

    public static void C_RoomListHandler(Session s, IPacket pkt)
    {
        ClientSession session = s as ClientSession;
        S_RoomList packet = new S_RoomList();

        Dictionary<int, Room> dic = Program.roomManager.GetRoomDic();
        foreach (KeyValuePair<int, Room> pair in dic)
        {
            packet.roomList.Add(new S_RoomList.Room() { roomId = pair.Value.roomId, roomName = pair.Value.roomName });
        }

        session.Send(packet.Write());
    }

    public static void C_MovePacketHandler(Session s, IPacket pkt)
    {
        ClientSession session = s as ClientSession;
        C_MovePacket packet = pkt as C_MovePacket;
        
        session.room.Push(() => session.room.Move(session, packet));
    }
}
