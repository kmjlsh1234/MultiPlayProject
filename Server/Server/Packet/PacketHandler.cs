using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PacketHandler
{
    public static void C_ChatHandler(Session session, IPacket packet)
    {
        C_Chat c_chat = packet as C_Chat;
        Console.WriteLine($"message : {c_chat.message}");


    }
    
    public static void C_EnterRoomHandler(Session session, IPacket packet)
    {
        
        //패킷 파싱
        ClientSession clientSession = session as ClientSession;
        C_EnterRoom c_EnterRoom = packet as C_EnterRoom;

        Console.WriteLine($"sessionId : {clientSession.sessionId} enter room!");
        //룸 입장 처리
        clientSession.room = Program.room;
        Program.room.EnterRoom(clientSession);

        //입장 유저에게 리스트 보내기
        S_PlayerList s_playerList = new S_PlayerList();
        s_playerList.playerList = clientSession.room.getPlayerList();
        Console.WriteLine($"playerList Count : {s_playerList.playerList.Count}");
        clientSession.Send(s_playerList.Write());


        //기존 입장한 사람에게 알리기
        S_BroadCast_EnterRoom s_BroadCast_EnterRoom = new S_BroadCast_EnterRoom();
        s_BroadCast_EnterRoom.sessionId = (ushort)clientSession.sessionId;
        clientSession.room.BroadCast(s_BroadCast_EnterRoom.Write());
    }

    public static void TestPacketHandler(Session session, IPacket packet)
    {
        TestPacket testPacket = packet as TestPacket;
        Console.WriteLine($"playerId : {testPacket.playerId}");
        Console.WriteLine($"message: {testPacket.message}");
    }
}
