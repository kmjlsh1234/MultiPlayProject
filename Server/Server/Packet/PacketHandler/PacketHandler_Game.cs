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

    public static void C_EnemymoveHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Enemymove packet = pkt as C_Enemymove;
        session.gameRoom.HandleEnemyMove(session, packet);
    }
}
