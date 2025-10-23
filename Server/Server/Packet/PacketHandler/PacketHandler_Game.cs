using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using ServerCore;

public partial class PacketHandler
{
    public static void C_LoadingCompleteHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;

        GameRoom room = session.gameRoom;
        if(room == null ) { return; }

        room.Push(room.CheckGameStart, session);

    }

    public static void C_ExitGameRoomHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;

        GameRoom room = session.gameRoom;
        if (room == null) { return; }

        room.Push(room.ExitRoom, session);
    }

    public static void C_MoveHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Move packet = pkt as C_Move;

        GameRoom room = session.gameRoom;
        if (room == null) { return; }

        room.Push(room.HandleMove, session, packet);
    }

    public static void C_InputHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Input packet = pkt as C_Input;

        GameRoom room = session.gameRoom;
        if (room == null) { return; }

        room.Push(room.HandleInput, session, packet);
    }

    public static void C_EnemyMoveHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_EnemyMove packet = pkt as C_EnemyMove;

        GameRoom room = session.gameRoom;
        if (room == null) { return; }

        room.Push(room.HandleEnemyMove, session, packet);
        
    }

    public static void C_ExpHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Exp packet = pkt as C_Exp;

        GameRoom room = session.gameRoom;
        if (room == null) { return; }

        room.Push(room.AddExp, session, packet);
    }

    public static void C_NewSkillHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_NewSkill packet = pkt as C_NewSkill;

        GameRoom room = session.gameRoom;
        if (room == null) { return; }

        room.Push(room.SkillSelect, session, packet);
    }

    public static void C_UpgradeSkillHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_UpgradeSkill packet = pkt as C_UpgradeSkill;

        GameRoom room = session.gameRoom;
        if (room == null) { return; }

        room.Push(room.SkillSelect, session, packet);
    }

    public static void C_SkillHandler(Session s, IMessage pkt)
    {
        ClientSession session = s as ClientSession;
        C_Skill packet = pkt as C_Skill;

        GameRoom room = session.gameRoom;
        if (room == null) { return; }

        room.Push(room.SkillAttack, session, packet);
    }
}
