using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using UnityEngine;

public partial class PacketHandler
{
    public static void S_IngamestartHandler(Session session, IMessage pkt)
    {
        Debug.Log(" S_InGameStart");
        LoadingSceneManager.Instance.OnLoadingCompleted.Invoke();
    }

    public static void S_GameroominfoHandler(Session s, IMessage pkt)
    {
        S_Gameroominfo packet = pkt as S_Gameroominfo;
        GameManager.Instance.GameStart(packet);
    }

    public static void S_ExitgameroomHandler(Session s, IMessage pkt)
    {
        S_Exitgameroom packet = pkt as S_Exitgameroom;
        GameManager.Instance.RemovePlayer(packet.SessionId);
    }

    public static void S_MoveHandler(Session session, IMessage pkt)
    {
        ServerSession serverSession = session as ServerSession;
        S_Move packet = pkt as S_Move;
        GameManager.Instance.PlayerMove(packet);
    }

    public static void S_SpawnenemyHandler(Session session, IMessage pkt)
    {
        S_Spawnenemy packet = pkt as S_Spawnenemy;
        GameManager.Instance.SpawnEnemy(packet);
    }

    public static void S_EnemymoveHandler(Session session, IMessage pkt)
    {
        if(GameManager.Instance.isMaster)
        {
            return;
        }
        S_Enemymove packet = pkt as S_Enemymove;
        GameManager.Instance.LerpEnemyPos(packet); 
    }
}
