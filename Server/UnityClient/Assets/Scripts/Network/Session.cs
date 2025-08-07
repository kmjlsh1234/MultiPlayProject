using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
namespace ServerCore
{
    public abstract class Session
    {
        Socket socket;
        Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>();
        List<ArraySegment<byte>> pendingList = new List<ArraySegment<byte>>();
        int disconnect = 0;
        

        object _lock = new object();
        SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();

        RecvBuffer recvBuffer = new RecvBuffer(1024);

        #region
        public abstract void OnConnected(EndPoint endPoint);
        public abstract void OnDisconnected(EndPoint endPoint);
        public abstract void OnRecvPacket(ArraySegment<byte> buffer);
        
        #endregion

        public void Init(Socket socket)
        {
            this.socket = socket;

            recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
            
            RegistRecv();
        }

        public void Send(ArraySegment<byte> sendBuffer)
        {
            lock (_lock)
            {
                //일단 Send요청이 들어오면 sendqueue에 담음
                sendQueue.Enqueue(sendBuffer);

                if(pendingList.Count == 0)
                {
                    RegistSend();
                }
            }
        }

        public void Disconnect()
        {
            
            if (Interlocked.Exchange(ref disconnect, 1) == 1)
            {
                return;
            }

            OnDisconnected(socket.RemoteEndPoint);

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            lock (_lock)
            {
                sendQueue.Clear();
                pendingList.Clear();
            }
            
        }

        #region 네트워크

        void RegistSend()
        {
            while (sendQueue.Count > 0)
            {
                pendingList.Add(sendQueue.Dequeue());
            }
            sendArgs.BufferList = pendingList.ToArray();

            bool pending = socket.SendAsync(sendArgs);
            if (pending == false)
            {
                OnSendCompleted(null, sendArgs);
            }
        }

        void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success && args.BytesTransferred > 0)
            {
                lock (_lock)
                {
                    sendArgs.BufferList = null;
                    pendingList.Clear();
                    if (sendQueue.Count > 0)
                    {
                        RegistSend();
                    }
                }
            }
            else
            {
                Disconnect();
            }
            
           
        }

        void RegistRecv()
        {
            //RecvBuffer 정리
            recvBuffer.Clean();

            //RecvBuffer에서 사용 가능 영역 가져오기
            ArraySegment<byte> buffer = recvBuffer.getAvailableArea();
            recvArgs.SetBuffer(buffer);
             
            bool pending = socket.ReceiveAsync(recvArgs);
            if (pending == false)
            {
                OnRecvCompleted(null, recvArgs);
            }
        }

        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {           
            if(args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                //버퍼 커서 이동
                bool isWriteSuccess = recvBuffer.OnWrite(args.BytesTransferred);
                if (!isWriteSuccess)
                {
                    Disconnect();
                }

                //얼마나 처리했는지 확인
                int readLen = OnRecv(recvBuffer.getReadArea());

                //Read커서 이동
                bool isReadSuccess = recvBuffer.OnRead(readLen);
                if (!isReadSuccess)
                {
                    Disconnect();
                }
                RegistRecv();

            }
            else
            {
                Disconnect();
            }  
        }

        int OnRecv(ArraySegment<byte> buffer)
        {
            int packetHeaderSize = 2;
            int readLen = 0;
            while (true)
            {
                //패킷 헤더도 파싱 못하는 경우
                if (buffer.Count < packetHeaderSize)
                {
                    break;
                }

                //패킷 사이즈 파싱
                ushort packetSize = BitConverter.ToUInt16(buffer);

                //패킷 사이즈가 버퍼보다 클 경우 => 덜옴
                if (packetSize > buffer.Count)
                {
                    break;
                }

                //패킷 처리
                OnRecvPacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, packetSize));
                readLen += packetSize;
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + packetSize, buffer.Count - packetSize);
            }

            return readLen;
        }

        
        #endregion
    }
}
