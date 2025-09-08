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

    public override void Init()
    {
        //IPAddress.Parse("10.153.33.245")
        //IPAddress.Loopback
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("10.153.33.245"), 8888);
        connector.Init(endPoint, session);
    }

    public void Send(IMessage packet)
    {
        session.Send(packet);
    }

    /// <summary>
    /// Unity에서는 UnityEngine API는 백그라운드 스레드에서 접근 못하기때문에 메인 스레드에서만 접근해야함
    /// </summary>
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
