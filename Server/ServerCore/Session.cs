using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public abstract class Session
    {
        Socket socket;
        Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>();
        List<ArraySegment<byte>> pendingList = new List<ArraySegment<byte>>();

        

        object _lock = new object();
        SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();

        RecvBuffer recvBuffer = new RecvBuffer(1024);

        public void Init(Socket socket)
        {
            this.socket = socket;
            Console.WriteLine($"Session 생성");

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

                if(sendQueue.Count == 0)
                {
                    RegistSend();
                }
            }
        }

        #region 네트워크

        void RegistSend()
        {
            
            while(sendQueue.Count > 0)
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

        void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            lock (_lock)
            {
                Console.WriteLine("SendComplete");
                if (sendQueue.Count > 0)
                {
                    RegistSend();
                }
            }
           
        }

        void RegistRecv()
        {
            //RecvBuffer 정리
            recvBuffer.Clean();

            //RecvBuffer에서 사용 가능 영역 가져오기
            ArraySegment<byte> buffer = recvBuffer.AvailableWriteArea;
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
                string data = Encoding.Unicode.GetString(args.Buffer, 0, args.BytesTransferred);
                Console.WriteLine(data + Environment.NewLine);
            }

            RegistRecv();
        }

        #endregion
    }
}
