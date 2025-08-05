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
        Func<Session> sessionFactory;


        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory)
        {
            this.sessionFactory = sessionFactory;

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
                Session session = sessionFactory.Invoke();
                session.Init(args.ConnectSocket);

                string data = "hello world!";
                byte[] buffer = Encoding.Unicode.GetBytes(data);

                for(int i=0; i<10; i++)
                {

                    session.Send(buffer);
                }
            }

            Console.WriteLine("Connect Complete!");
        }
    }
}
