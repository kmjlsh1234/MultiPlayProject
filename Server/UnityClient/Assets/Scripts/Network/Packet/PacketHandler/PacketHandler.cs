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
        RoomManager.Instance.RecevMessage(chat);
    }

    //로딩 시작
    

    //인게임 전환
    public static void S_IngamestartHandler(Session session, IMessage pkt)
    {
        Debug.Log(" S_InGameStart");
        LoadingSceneManager.Instance.OnLoadingCompleted.Invoke();
    }

    
}
