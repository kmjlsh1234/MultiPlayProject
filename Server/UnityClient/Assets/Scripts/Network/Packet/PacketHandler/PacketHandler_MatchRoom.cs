using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using UnityEngine;

public partial class PacketHandler
{
    public static void S_RoominfoHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_RoomInfo");

        S_Roominfo packet = pkt as S_Roominfo;

        RoomManager.Instance.InitRoom(packet);
        UIManager.Instance.Push(UIType.UIPopup_Match);
    }

    public static void S_EntermatchroomHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_EnterRoom");
        S_Entermatchroom packet = pkt as S_Entermatchroom;

        if (packet.PlayerInfo.SessionId == NetworkManager.Instance.sessionId)
        {
            return;
        }


        RoomManager.Instance.AddPlayer(packet.PlayerInfo);
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
            RoomManager.Instance.RemovePlayer(packet.SessionId);
        }
    }

    public static void S_ChangeroominfoHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_ChangeRoomInfo");

        S_Changeroominfo packet = pkt as S_Changeroominfo;

        RoomManager.Instance.UpdateMaster(packet.MasterId);
    }

    public static void S_ReadyHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_BroadCast_ReadyPacket");
        S_Ready packet = pkt as S_Ready;

        RoomManager.Instance.UpdateReady(packet.SessionId, packet.IsReady);
    }
}
