using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class PacketHandler
{
    public static void S_ConnectHandler(Session s, IMessage pkt)
    {
        Debug.Log("S_ConnectHandler");
        S_Connect packet = pkt as S_Connect;
        NetworkManager.Instance.sessionId = packet.SessionId;
        SceneManager.LoadScene(SceneType.LobbyScene.ToString());
    }

    public static void S_InviteHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_InvitePacket");
        S_Invite packet = pkt as S_Invite;
        UIManager.Instance.Push(UIType.UIPopup_Invite, packet);
    }
}
