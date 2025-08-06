using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace TestClient
{
    public class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"Server Connected!");

            TestPacket testPacket = new TestPacket();
            testPacket.packetId = 0;
            testPacket.playerId = 4;
            testPacket.message = "hello world";

            ArraySegment<byte> buffer = testPacket.Write();


            for (int i = 0; i < 10; i++)
            {

                Send(buffer);
            }
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"Server DisConnected!");
        }
    }
}
