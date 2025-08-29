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

public partial class PacketHandler
{
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

    

    public static void C_StartHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
    }

    public static void C_LoadingcompleteHandler(Session s, IMessage pkt)
    {        
        ClientSession session = s as ClientSession;
        //session.room.Push(() => session.room.LoadingComplete(session));

    }

    
}
