using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using ServerCore;

public partial class PacketHandler
{
    public static void C_ExitgameroomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        session.gameRoom.ExitRoom(session);
    }

    public static void C_MoveHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Move packet = pkt as C_Move;
        session.gameRoom.HandleMove(session, packet);
    }
}
