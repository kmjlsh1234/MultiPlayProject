using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PacketHandler
{
    public static void S_PongHandler(Session s, IMessage pkt)
    {

    }

    public static void S_ConnectHandler(Session s, IMessage pkt) 
    {
        Debug.Log("S_ConnectHandler");
        S_Connect packet = pkt as S_Connect;
        NetworkManager.Instance.sessionId = packet.SessionId;
        SceneManager.LoadScene(SceneType.LobbyScene.ToString());
    }

    #region :::: Room
    public static void S_EnterroomHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_EnterRoom");
        ServerSession serverSession = session as ServerSession;
        S_Enterroom packet = pkt as S_Enterroom;

        if (packet.PlayerInfo.SessionId == NetworkManager.Instance.sessionId)
        {
            return;
        }

        
        ChatManager.Instance.AddPlayer(packet.PlayerInfo);
    }

    public static void S_ExitroomHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_ExitRoom");
        ServerSession serverSession = session as ServerSession;
        S_Exitroom packet = pkt as S_Exitroom;

        if (NetworkManager.Instance.sessionId == packet.SessionId)
        {
            UIManager.Instance.Pop();
            NetworkManager.Instance.sessionId = 0;
        }
        else
        {
            ChatManager.Instance.RemovePlayer(packet.SessionId);
        }
    }

    public static void S_ChangeroominfoHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_ChangeRoomInfo");

        S_Changeroominfo packet = pkt as S_Changeroominfo;
        
        ChatManager.Instance.ChangeRoomInfo(packet);
    }

    #endregion
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

    

    public static void S_RoominfoHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_RoomInfo");

        S_Roominfo packet = pkt as S_Roominfo;
        
        ChatManager.Instance.OnPlayerListRecv(packet);
        UIManager.Instance.Push(UIType.UIPopup_Match);
    }

    

    public static void S_RoomlistHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_RoomList");

        S_Roomlist packet = pkt as S_Roomlist;
        DataManager.Instance.OnRoomListRecvCompleted(packet);
    }

    public static void S_ErrorcodeHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_ErrorCode");

        ServerSession serverSession = session as ServerSession;
        S_Errorcode packet = pkt as S_Errorcode;

        UIManager.Instance.Push(UIType.UIPopup_Error, packet);
    }

    public static void S_MoveHandler(Session session, IMessage pkt)
    {
        ServerSession serverSession = session as ServerSession;
        S_Move packet = pkt as S_Move;

        PlayerManager.Instance.OnPacketRecv(packet);
    }

    public static void S_ReadyHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_ReadyPacket");

        ServerSession serverSession = session as ServerSession;
        S_Ready packet = pkt as S_Ready;

        ChatManager.Instance.OnBroadCastReadyPacketRecv(packet.SessionId, packet.IsReady);
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

    public static void S_InviteHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_InvitePacket");
        S_Invite packet = pkt as S_Invite;
        UIManager.Instance.Push(UIType.UIPopup_Invite, packet);
    }

    public static void S_SpawnenemyHandler(Session session, IMessage pkt)
    {
        S_Spawnenemy packet = pkt as S_Spawnenemy;
        GameManager.Instance.SpawnEnemy(packet);
    }
}
