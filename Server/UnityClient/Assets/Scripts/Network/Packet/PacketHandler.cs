using ServerCore;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PacketHandler
{
    public static void S_BroadCast_ChatHandler(Session session, IPacket pkt)
    {
        ServerSession serverSession = session as ServerSession;
        S_BroadCast_Chat packet = pkt as S_BroadCast_Chat;

        Chat chat = new Chat()
        {
            playerId = packet.sessionId,
            message = packet.message,
        };
        ChatManager.Instance.RecevMessage(chat);
    }

    public static void S_BroadCast_EnterRoomHandler(Session session, IPacket pkt)
    {
        Debug.Log("S_BroadCast_EnterRoomHandler");
        ServerSession serverSession = session as ServerSession;
        S_BroadCast_EnterRoom packet = pkt as S_BroadCast_EnterRoom;

        if(packet.sessionId == NetworkManager.Instance.sessionId)
        { 
            return;
        }

        Player player = new Player() { playerId = packet.sessionId };
        ChatManager.Instance.AddPlayer(player);
    }

    public static void S_PlayerListHandler(Session session, IPacket pkt)
    {
        
        S_PlayerList packet = pkt as S_PlayerList;
        Dictionary<int, Player> playerDic = new Dictionary<int, Player>();

        foreach (var p in packet.playerList)
        {
            Player player = new Player()
            {
                playerId = p.sessionId,
                isSelf = p.isSelf,
            };

            if (p.isSelf)
            {
                NetworkManager.Instance.sessionId = p.sessionId;

            }
            playerDic.Add(player.playerId, player);
        }

        ChatManager.Instance.OnPlayerListRecv(playerDic);
        UIManager.Instance.Push(UIType.UIPopup_Chat);
    }

    public static void S_BroadCast_ExitRoomHandler(Session session, IPacket pkt)
    {
        ServerSession serverSession = session as ServerSession;
        S_BroadCast_ExitRoom packet = pkt as S_BroadCast_ExitRoom;

        if(NetworkManager.Instance.sessionId == packet.sessionId){
            UIManager.Instance.Pop();
            NetworkManager.Instance.sessionId = 0;
        }
        else
        {
            ChatManager.Instance.RemovePlayer(packet.sessionId);
        }
    }

    public static void S_RoomListHandler(Session session, IPacket pkt)
    {
        ServerSession serverSession = session as ServerSession;
        S_RoomList packet = pkt as S_RoomList;
        
        Dictionary<int, Room> dic = new Dictionary<int, Room>();
        foreach(var p in packet.roomList)
        {
            dic.Add(p.roomId, new Room() { roomId = p.roomId, roomName = p.roomName });
        }
        
        RoomManager.Instance.OnRoomListRecv(dic);
    }

    public static void S_ErrorCodeHandler(Session session, IPacket pkt)
    {
        ServerSession serverSession = session as ServerSession;
        S_ErrorCode packet = pkt as S_ErrorCode;

        UIManager.Instance.Push(UIType.UIPopup_Error, packet);
    }
}
