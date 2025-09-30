using Google.Protobuf;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PacketHandler
{
    public static void C_PingCheckHandler(Session s, IMessage pkt)
    {
        //s.Send(new S_PongPacket().Write());
    }
}
