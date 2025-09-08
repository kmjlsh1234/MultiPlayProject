using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using UnityEngine;

public partial class PacketHandler
{
    public static Action<S_Matchroominfo> Event_S_MatchRoomInfo;
    public static Action<Matchplayerinfo> Event_S_EnterMatchRoom;
    public static Action<S_Exitroom> Event_S_ExitRoom;
    public static Action<S_Ready> Event_S_Ready;

    public static void S_MatchroominfoHandler(Session session, IMessage pkt)
    {
        S_Matchroominfo packet = pkt as S_Matchroominfo;

        UIManager.Instance.Push(UIType.UIPopup_Match, packet);
        Event_S_MatchRoomInfo?.Invoke(packet);
    }

    public static void S_EntermatchroomHandler(Session session, IMessage pkt)
    {
        S_Entermatchroom packet = pkt as S_Entermatchroom;

        if (packet.PlayerInfo.SessionId.Equals(NetworkManager.Instance.sessionId))
        {
            return;
        }

        Event_S_EnterMatchRoom?.Invoke(packet.PlayerInfo);
    }

    public static void S_ExitroomHandler(Session session, IMessage pkt)
    {
        ServerSession serverSession = session as ServerSession;
        S_Exitroom packet = pkt as S_Exitroom;

        if (NetworkManager.Instance.sessionId == packet.SessionId)
        {
            UIManager.Instance.Pop();
            NetworkManager.Instance.sessionId = 0;
        }
        else
        {
            Event_S_ExitRoom?.Invoke(packet);
        }
    }

    public static void S_ReadyHandler(Session session, IMessage pkt)
    {
        S_Ready packet = pkt as S_Ready;
        Event_S_Ready?.Invoke(packet);
    }

    public static void S_LoadingstartHandler(Session session, IMessage pkt)
    {
        UIManager.Instance.Clear();
        LoadingSceneManager.Instance.LoadScene(SceneType.InGameScene);
    }
}