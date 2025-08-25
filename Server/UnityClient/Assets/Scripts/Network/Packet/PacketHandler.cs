using Google.Protobuf;
using ServerCore;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PacketHandler
{
    public static void S_PongHandler(Session s, IMessage pkt)
    {

    }

    public static void S_MovelobbyHandler(Session s, IMessage pkt) 
    { 
        S_MoveLobbyPacket packet = pkt as S_MoveLobbyPacket;
        NetworkManager.Instance.sessionId = packet.sessionId;
        SceneManager.LoadScene("LobbyScene");
        //LoadingSceneManager.Instance.LoadScene(SceneType.LobbyScene);
    }

    #region :::: Room
    public static void S_EnterroomHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_EnterRoom");
        ServerSession serverSession = session as ServerSession;
        S_BroadCast_EnterRoom packet = pkt as S_BroadCast_EnterRoom;

        if (packet.sessionId == NetworkManager.Instance.sessionId)
        {
            return;
        }

        PlayerData playerData = new PlayerData() 
        { 
            sessionId = packet.sessionId,
            nickName = packet.nickName,
        };
        ChatManager.Instance.AddPlayer(playerData);
    }

    public static void S_ExitroomHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_ExitRoom");
        ServerSession serverSession = session as ServerSession;
        S_BroadCast_ExitRoom packet = pkt as S_BroadCast_ExitRoom;

        if (NetworkManager.Instance.sessionId == packet.sessionId)
        {
            UIManager.Instance.Pop();
            NetworkManager.Instance.sessionId = 0;
        }
        else
        {
            ChatManager.Instance.RemovePlayer(packet.sessionId);
        }
    }

    public static void S_ChangeroominfoHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_ChangeRoomInfo");

        S_BroadCast_ChangeRoomInfo packet = pkt as S_BroadCast_ChangeRoomInfo;
        RoomData roomData = new RoomData()
        {
            roomId = packet.roomId,
            roomName = packet.roomName,
            masterId = packet.masterId,

        };
        ChatManager.Instance.ChangeRoomInfo(roomData);
    }

    #endregion
    public static void S_ChatHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_Chat");
        ServerSession serverSession = session as ServerSession;
        S_BroadCast_Chat packet = pkt as S_BroadCast_Chat;

        Chat chat = new Chat()
        {
            playerId = packet.sessionId,
            message = packet.message,
        };
        ChatManager.Instance.RecevMessage(chat);
    }

    

    public static void S_RoominfoHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_RoomInfo");

        S_RoomInfo packet = pkt as S_RoomInfo;
        Dictionary<int, PlayerData> playerDic = new Dictionary<int, PlayerData>();

        RoomData roomData = new RoomData()
        {
            roomId = packet.roomId,
            roomName = packet.roomName,
            masterId = packet.masterId,
        };

        foreach (var p in packet.playerList)
        {
            PlayerData playerData = new PlayerData()
            {
                sessionId = p.sessionId,
                nickName = p.nickName,
                isSelf = p.isSelf,
                isMaster = p.isMaster,
                isReady = p.isReady,
            };

            if (p.isSelf)
            {
                NetworkManager.Instance.sessionId = p.sessionId;
            }
            playerDic.Add(playerData.sessionId, playerData);
        }

        ChatManager.Instance.OnPlayerListRecv(roomData, playerDic);
        UIManager.Instance.Push(UIType.UIPopup_Match);
    }

    

    public static void S_RoomlistHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_RoomList");

        S_RoomList packet = pkt as S_RoomList;
        
        Dictionary<int, RoomData> dic = new Dictionary<int, RoomData>();
        foreach(var p in packet.roomList)
        {
            dic.Add(p.roomId, new RoomData() 
            { 
                roomId = p.roomId, 
                roomName = p.roomName,
            });
        }
        
        DataManager.Instance.OnRoomListRecvCompleted(dic);
    }

    public static void S_ErrorcodeHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_ErrorCode");

        ServerSession serverSession = session as ServerSession;
        S_ErrorCode packet = pkt as S_ErrorCode;

        UIManager.Instance.Push(UIType.UIPopup_Error, packet);
    }

    public static void S_MoveHandler(Session session, IMessage pkt)
    {
        ServerSession serverSession = session as ServerSession;
        S_BroadCast_MovePacket packet = pkt as S_BroadCast_MovePacket;

        PlayerManager.Instance.OnPacketRecv(packet);
    }

    public static void S_ReadyHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_ReadyPacket");

        ServerSession serverSession = session as ServerSession;
        S_BroadCast_ReadyPacket packet = pkt as S_BroadCast_ReadyPacket;

        ChatManager.Instance.OnBroadCastReadyPacketRecv(packet.sessionId, packet.isReady);
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
        S_InvitePacket packet = pkt as S_InvitePacket;
        UIManager.Instance.Push(UIType.UIPopup_Invite, packet);
    }

    public static void S_SpawnenemyHandler(Session session, IMessage pkt)
    {
        S_BroadCast_SpawnEnemy packet = pkt as S_BroadCast_SpawnEnemy;
        GameManager.Instance.SpawnEnemy(packet);
    }
}
