using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class PacketHandler
{
    public static void S_ChatHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_Chat");
        ServerSession serverSession = session as ServerSession;
        S_Chat packet = pkt as S_Chat;

        Chat chat = new Chat()
        {
            playerId = packet.SessionId,
            message = packet.Message,
        };
        ChatManager.Instance.RecevMessage(chat);
    }

    public static void S_MoveHandler(Session session, IMessage pkt)
    {
        ServerSession serverSession = session as ServerSession;
        S_Move packet = pkt as S_Move;

        PlayerManager.Instance.OnPacketRecv(packet);
    }

    //로딩 시작
    public static void S_LoadingstartHandler(Session session, IMessage pkt)
    {
        Debug.Log(" S_BroadCast_LoadingStartPacket");

        UIManager.Instance.Clear();
        LoadingSceneManager.Instance.LoadScene(SceneType.InGameScene);
    }

    //인게임 전환
    public static void S_IngamestartHandler(Session session, IMessage pkt)
    {
        Debug.Log(" S_InGameStart");
        LoadingSceneManager.Instance.OnLoadingCompleted.Invoke();
    }

    public static void S_SpawnenemyHandler(Session session, IMessage pkt)
    {
        S_Spawnenemy packet = pkt as S_Spawnenemy;
        GameManager.Instance.SpawnEnemy(packet);
    }
}
