using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Listener
    {
        Socket listenSocket;
        Func<Session> sessionFactory;

        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory)
        {
            this.sessionFactory = sessionFactory;

            listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(endPoint);
            listenSocket.Listen(10);

            Console.WriteLine("Server is Listening ... ");

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            RegistAccept(args);
        }

        private void RegistAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            bool pending = listenSocket.AcceptAsync(args);
            //즉시 완료되면 args.OnCompleted가 호출 안됨
            //SocketAsyncEventArgs 기반 비동기 메서드는 즉시 완료되면 이벤트를 호출하지 않는다
            if (pending == false)
            {
                Console.WriteLine("AcceptAsync가 즉시 완료됨");
                OnAcceptCompleted(null, args);
            }
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success)
            {
                Session session = sessionFactory.Invoke();
                session.Init(args.AcceptSocket);
            }

            RegistAccept(args);
            Console.WriteLine("다시 Accept 등록");
        }
    }
}
