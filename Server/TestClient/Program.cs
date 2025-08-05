using System.Net;
using System.Net.Sockets;
using ServerCore;

namespace TestClient
{
    public class Program
    {
        static Connector connector = new Connector();

        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 8888);
            connector.Init(endPoint, () => { return new ServerSession(); });

            while (true)
            {

            }
        }
    }
}
