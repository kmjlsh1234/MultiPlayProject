using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using ServerCore;

public partial class PacketHandler
{
    public static void C_LoadingcompleteHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        session.gameRoom.Push(() => session.gameRoom.CheckGameStart(session));

    }

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

    public static void C_InputHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Input packet = pkt as C_Input;
        session.gameRoom.HandleInput(session, packet);
    }

    public static void C_EnemymoveHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Enemymove packet = pkt as C_Enemymove;
        session.gameRoom.HandleEnemyMove(session, packet); 
    }

    public static void C_ExpHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Exp packet = pkt as C_Exp;
        session.gameRoom.Push(() => session.gameRoom.AddExp(session, packet));
    }

    public static void C_NewskillHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Newskill packet = pkt as C_Newskill;
        session.gameRoom.Push(() => session.gameRoom.SkillSelect(session, packet));
    }

    public static void C_UpgradeskillHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Upgradeskill packet = pkt as C_Upgradeskill;
        session.gameRoom.Push(() => session.gameRoom.SkillSelect(session, packet));
    }
}
