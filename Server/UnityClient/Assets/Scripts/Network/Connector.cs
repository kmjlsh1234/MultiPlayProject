using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Connector
    {
        Session session;

        public void Init(IPEndPoint endPoint, Session session)
        {
            this.session = session;

            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.UserToken = socket;
            args.RemoteEndPoint = endPoint;
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectCompleted);

            RegistConnect(args);
        }

        void RegistConnect(SocketAsyncEventArgs args)
        {
            Socket socket = args.UserToken as Socket;

            bool pending = socket.ConnectAsync(args);
            if(pending == false)
            {
                OnConnectCompleted(null, args);
            }
        }

        void OnConnectCompleted(object sender, SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success)
            {
                session.Init(args.ConnectSocket);
            }
        }
    }
}
