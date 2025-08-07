using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    #region �̱���
    public static NetworkManager Instance { get; private set; }
    #endregion

    ServerSession session = new ServerSession();

    Connector connector = new Connector();
    Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // �ߺ� ����
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // �ʿ��ϸ�
    }

    void Start()
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
        
    }
}
