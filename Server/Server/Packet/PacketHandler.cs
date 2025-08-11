using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PacketHandler
{
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
        Console.WriteLine($"sessionId : {session.sessionId} enter room!");

        //룸 입장 처리
        session.room = Program.room;
        Program.room.Push(() => session.room.EnterRoom(session));
    }

    public static void TestPacketHandler(Session s, IPacket pkt)
    {
        TestPacket packet = pkt as TestPacket;
        Console.WriteLine($"playerId : {packet.playerId}");
        Console.WriteLine($"message: {packet.message}");
    }

    public static void C_ExitRoomHandler(Session s, IPacket pkt)
    {
        ClientSession session = s as ClientSession;
        C_ExitRoom packet = pkt as C_ExitRoom;

        Program.room.Push(() => session.room.ExitRoom(session));
    }
}
