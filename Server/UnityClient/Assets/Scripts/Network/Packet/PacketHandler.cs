using ServerCore;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    public static void S_BroadCast_ChangeMasterHandler(Session session, IPacket pkt)
    {
        
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

    

    public static void S_PlayerListHandler(Session session, IPacket pkt)
    {
        Debug.Log("S_PlayerListHandler");

        S_PlayerList packet = pkt as S_PlayerList;
        Dictionary<int, PlayerData> playerDic = new Dictionary<int, PlayerData>();

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

        ChatManager.Instance.OnPlayerListRecv(playerDic);
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
        
        RoomManager.Instance.OnRoomListRecv(dic);
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

    public static void S_ReadyCheckPacketHandler(Session session, IPacket pkt)
    {
        Debug.Log("S_ReadyCheckPacketHandler");

        ServerSession serverSession = session as ServerSession;
        S_ReadyCheckPacket packet = pkt as S_ReadyCheckPacket;


    }
}
