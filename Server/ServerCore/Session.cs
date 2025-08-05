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
        
        SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();

        public void Init(Socket socket)
        {
            this.socket = socket;
            Console.WriteLine($"Session 생성");

            recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

            RegistRecv();
        }


        #region 네트워크

        void RegistSend()
        {
            
        }

        void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {

        }

        void RegistRecv()
        {
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

                Console.WriteLine(args.Buffer);
            }

            RegistRecv();
        }

        #endregion
    }
}
