using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class NetworkManager : SingletonBase<NetworkManager>
{
    ServerSession session = new ServerSession();

    Connector connector = new Connector();
    Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>();

    

    public void Init()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 8888);
        connector.Init(endPoint, session);
    }

    public void Send(ArraySegment<byte> buff)
    {
        session.Send(buff);
    }

    void Update()
    {
        List<IPacket> packets = PacketQueue.Instance.PopAll();
        foreach (IPacket packet in packets)
        {
            PacketManager.Instance.HandlePacket(session, packet);
        }
    }
}
