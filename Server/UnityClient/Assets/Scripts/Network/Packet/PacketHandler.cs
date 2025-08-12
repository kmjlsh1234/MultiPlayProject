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
        Debug.Log("S_BroadCast_ChatHandler");
        ChatManager.Instance.RecevMessage(chat);
    }

    public static void S_BroadCast_EnterRoomHandler(Session session, IPacket pkt)
    {
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
        List<Player> list = new List<Player>();

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
            list.Add(player);
        }

        ChatManager.Instance.playerList = list;
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

    }
}
