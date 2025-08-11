using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PacketQueue
{
    private static PacketQueue instance;

    public static PacketQueue Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new PacketQueue();
            }
            return instance;
        }
    }

    Queue<IPacket> packetQueue = new Queue<IPacket> ();
    object key = new object ();

    public void Push(IPacket packet)
    {
        lock (key)
        {
            packetQueue.Enqueue (packet);
        }
    }

    public List<IPacket> PopAll()
    {
        List<IPacket> packets = new List<IPacket> ();
        lock (key)
        {
            while(packetQueue.Count > 0)
            {
                packets.Add (packetQueue.Dequeue());
            }
        }

        return packets;
    }
}
