using Google.Protobuf;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class NetworkManager : SingletonBase<NetworkManager>
{
    public int sessionId;
    public string uuid;

    ServerSession session = new ServerSession();

    Connector connector = new Connector();
    Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>();

    

    public override void Init()
    {
        //IPAddress.Parse("10.153.33.245")
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 8888);
        connector.Init(endPoint, session);
    }

    public void Send(IMessage packet)
    {
        session.Send(packet);
    }

    void Update()
    {
        List<PacketMessage> packets = PacketQueue.Instance.PopAll();
        foreach (PacketMessage packet in packets)
        {
            Action<Session, IMessage> handler = PacketManager.Instance.GetPacketHandler(packet.id);
            if(handler != null)
            {
                handler.Invoke(session, packet.message);
            }
        }
    }
}
