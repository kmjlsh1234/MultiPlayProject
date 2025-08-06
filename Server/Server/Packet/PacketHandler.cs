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

    }

    public static void TestPacketHandler(Session session, IPacket packet)
    {
        TestPacket testPacket = packet as TestPacket;
        Console.WriteLine($"playerId : {testPacket.playerId}");
        Console.WriteLine($"message: {testPacket.message}");
    }
}
