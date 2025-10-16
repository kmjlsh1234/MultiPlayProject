using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using ServerCore;

public partial class PacketHandler
{
    public static void C_LoadingCompleteHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        session.gameRoom.Push(() => session.gameRoom.CheckGameStart(session));

    }

    public static void C_ExitGameRoomHandler(Session s, IMessage pkt)
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

    public static void C_EnemyMoveHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_EnemyMove packet = pkt as C_EnemyMove;
        session.gameRoom.HandleEnemyMove(session, packet); 
    }

    public static void C_ExpHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Exp packet = pkt as C_Exp;
        session.gameRoom.Push(() => session.gameRoom.AddExp(session, packet));
    }

    public static void C_NewSkillHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_NewSkill packet = pkt as C_NewSkill;
        session.gameRoom.Push(() => session.gameRoom.SkillSelect(session, packet));
    }

    public static void C_UpgradeSkillHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_UpgradeSkill packet = pkt as C_UpgradeSkill;
        session.gameRoom.Push(() => session.gameRoom.SkillSelect(session, packet));
    }

    public static void C_SkillHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Skill packet = pkt as C_Skill;
        session.gameRoom.Push(() => session.gameRoom.SkillAttack(session, packet));
    }
}
