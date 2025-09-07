using Google.Protobuf;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PacketMessage
{
    public ushort id { get; set; }
    public IMessage message { get; set; }
}

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

    Queue<PacketMessage> packetQueue = new Queue<PacketMessage> ();
    object key = new object ();

    public void Push(ushort id, IMessage packet)
    {
        lock (key)
        {
            packetQueue.Enqueue (new PacketMessage() { id = id, message = packet});
        }
    }

    public List<PacketMessage> PopAll()
    {
        List<PacketMessage> packets = new List<PacketMessage> ();
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
