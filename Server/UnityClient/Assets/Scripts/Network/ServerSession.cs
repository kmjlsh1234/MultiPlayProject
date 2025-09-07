using Google.Protobuf;
using Google.Protobuf.Protocol;
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
    public void Send(IMessage packet)
    {
        string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
        MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);
        ushort size = (ushort)packet.CalculateSize();
        byte[] sendBuffer = new byte[size + 4];
        Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
        Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
        Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);
        Send(new ArraySegment<byte>(sendBuffer));
    }

    public override void OnConnected(EndPoint endPoint)
    {
        UnityEngine.Debug.Log("OnConnected");
        PacketManager.Instance.CustomHandler = (session, message, id) =>
        {
            PacketQueue.Instance.Push(id, message);
        };
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        UnityEngine.Debug.Log("Server Disconnected");
        S_Errorcode errorCode = new S_Errorcode() { Code = 100, Message = "Server Disconnected"};
        UIManager.Instance.Push(UIType.UIPopup_Error, errorCode);
    }

    public override void OnRecvPacket(ArraySegment<byte> buffer)
    {
        PacketManager.Instance.OnRecvPacket(this, buffer);
    }
}
