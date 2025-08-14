using ServerCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class ServerSession : Session
{
    public override void OnConnected(EndPoint endPoint)
    {
        Console.WriteLine($"Server Connected!");
        UnityEngine.Debug.Log("Server Connected");
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        UnityEngine.Debug.Log("Server Disconnected");
        S_ErrorCode errorCode = new S_ErrorCode() { code = 100, message = "Server Disconnected"};
        UIManager.Instance.Push(UIType.UIPopup_Error, errorCode);
    }

    public override void OnRecvPacket(ArraySegment<byte> buffer)
    {
        PacketManager.Instance.OnRecvPacket(this, buffer, (s, p) => PacketQueue.Instance.Push(p));
    }
}
