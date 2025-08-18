using ServerCore;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PacketHandler
{
    #region :::: Room
    public static void S_BroadCast_EnterRoomHandler(Session session, IPacket pkt)
    {
        Debug.Log("S_BroadCast_EnterRoomHandler");
        ServerSession serverSession = session as ServerSession;
        S_BroadCast_EnterRoom packet = pkt as S_BroadCast_EnterRoom;

        if (packet.sessionId == NetworkManager.Instance.sessionId)
        {
            return;
        }

        PlayerData playerData = new PlayerData() { sessionId = packet.sessionId };
        ChatManager.Instance.AddPlayer(playerData);
    }

    public static void S_BroadCast_ExitRoomHandler(Session session, IPacket pkt)
    {
        Debug.Log("S_BroadCast_ExitRoomHandler");
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

    public static void S_BroadCast_ChangeRoomInfoHandler(Session session, IPacket pkt)
    {
        Debug.Log("S_BroadCast_ChangeRoomInfoHandler");

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
    public static void S_BroadCast_ChatHandler(Session session, IPacket pkt)
    {
        Debug.Log("S_BroadCast_ChatHandler");
        ServerSession serverSession = session as ServerSession;
        S_BroadCast_Chat packet = pkt as S_BroadCast_Chat;

        Chat chat = new Chat()
        {
            playerId = packet.sessionId,
            message = packet.message,
        };
        ChatManager.Instance.RecevMessage(chat);
    }

    

    public static void S_RoomInfoHandler(Session session, IPacket pkt)
    {
        Debug.Log("S_RoomInfoHandler");

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
        UIManager.Instance.Push(UIType.UIPopup_Chat);
    }

    

    public static void S_RoomListHandler(Session session, IPacket pkt)
    {
        Debug.Log("S_RoomListHandler");

        S_RoomList packet = pkt as S_RoomList;
        
        Dictionary<int, RoomData> dic = new Dictionary<int, RoomData>();
        foreach(var p in packet.roomList)
        {
            dic.Add(p.roomId, new RoomData() { roomId = p.roomId, roomName = p.roomName });
        }
        
        DataManager.Instance.OnRoomListRecvCompleted(dic);
    }

    public static void S_ErrorCodeHandler(Session session, IPacket pkt)
    {
        Debug.Log("S_ErrorCodeHandler");

        ServerSession serverSession = session as ServerSession;
        S_ErrorCode packet = pkt as S_ErrorCode;

        UIManager.Instance.Push(UIType.UIPopup_Error, packet);
    }

    public static void S_BroadCast_MovePacketHandler(Session session, IPacket pkt)
    {
        ServerSession serverSession = session as ServerSession;
        S_BroadCast_MovePacket packet = pkt as S_BroadCast_MovePacket;
    }

    public static void S_BroadCast_ReadyPacketHandler(Session session, IPacket pkt)
    {
        Debug.Log("S_BroadCast_ReadyPacketHandler");

        ServerSession serverSession = session as ServerSession;
        S_BroadCast_ReadyPacket packet = pkt as S_BroadCast_ReadyPacket;

        ChatManager.Instance.OnBroadCastReadyPacketRecv(packet.sessionId, packet.isReady);
    }

    public static void S_BroadCast_StartPacketHandler(Session session, IPacket pkt)
    {
        Debug.Log(" S_BroadCast_StartPacketHandler");

        UIManager.Instance.Clear();
        LoadingSceneManager.Instance.LoadScene(SceneType.Playground);
    }
}
