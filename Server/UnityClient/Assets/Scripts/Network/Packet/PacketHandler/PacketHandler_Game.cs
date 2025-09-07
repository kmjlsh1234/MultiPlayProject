using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using UnityEngine;

public partial class PacketHandler
{
    public static void S_ExitgameroomHandler(Session s, IMessage pkt)
    {
        Debug.Log("S_Exitgameroom");
        S_Exitgameroom packet = pkt as S_Exitgameroom;
        PlayerManager.Instance.RemovePlayer(packet.SessionId);
    }

    public static void S_LoadingstartHandler(Session session, IMessage pkt)
    {
        Debug.Log(" S_BroadCast_LoadingStartPacket");

        UIManager.Instance.Clear();
        LoadingSceneManager.Instance.LoadScene(SceneType.InGameScene);

    }

    public static void S_MoveHandler(Session session, IMessage pkt)
    {
        ServerSession serverSession = session as ServerSession;
        S_Move packet = pkt as S_Move;
        PlayerManager.Instance.OnPacketRecv(packet);
    }

    public static void S_SpawnenemyHandler(Session session, IMessage pkt)
    {
        Debug.Log(" S_Spawnenemy");
        S_Spawnenemy packet = pkt as S_Spawnenemy;
        GameManager.Instance.SpawnEnemy(packet);
    }

    public static void S_EnemymoveHandler(Session session, IMessage pkt)
    {
        Debug.Log(" S_Enemymove");
        if(NetworkManager.Instance.sessionId == RoomManager.Instance.masterId)
        {
            return;
        }
        S_Enemymove packet = pkt as S_Enemymove;
        GameManager.Instance.LerpEnemyPos(packet); 
    }
}
