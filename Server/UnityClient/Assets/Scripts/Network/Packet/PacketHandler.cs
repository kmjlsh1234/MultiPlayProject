using NUnit.Framework;
using ServerCore;
using System.Collections.Generic;
using UnityEngine;

public class PacketHandler
{
    public static void S_BroadCast_ChatHandler(Session session, IPacket packet)
    {

    }

    public static void S_BroadCast_EnterRoomHandler(Session session, IPacket packet)
    {
        ServerSession serverSession = session as ServerSession;
        S_BroadCast_EnterRoom s_BroadCast_EnterRoom = packet as S_BroadCast_EnterRoom;
    }

    public static void S_PlayerListHandler(Session session, IPacket packet)
    {
        
        S_PlayerList s_playerList = packet as S_PlayerList;
        List<S_PlayerList.Player> playerList = s_playerList.playerList;

        
    }
}
