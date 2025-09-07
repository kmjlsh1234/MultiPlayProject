using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using UnityEngine;

public partial class PacketHandler
{
    public static void S_PongcheckHandler(Session s, IMessage pkt)
    {

    }

    public static void S_ErrorcodeHandler(Session session, IMessage pkt)
    {
        Debug.Log("S_ErrorCode");

        ServerSession serverSession = session as ServerSession;
        S_Errorcode packet = pkt as S_Errorcode;

        UIManager.Instance.Push(UIType.UIPopup_Error, packet);
    }
}
