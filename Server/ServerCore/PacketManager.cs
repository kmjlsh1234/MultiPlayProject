using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class PacketManager
    {
        #region
        private static PacketManager instance;
        public static PacketManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new PacketManager();
                }
                return instance;
            }
        }
        #endregion

        Dictionary<ushort, Func<ArraySegment<byte>, IPacket>> makePacket = new Dictionary<ushort, Func<ArraySegment<byte>, IPacket>> ();
        Dictionary<ushort, Action<Session, IPacket>> handler = new Dictionary<ushort, Action<Session, IPacket>>();

        public void Register()
        {
            //makePacket.Add(PacketID.TestPacket, MakePacket<C_TestPacket>));
            //handler.Add(PacketID.TestPacket, PacketHandler.C_TestPacketHandler);
        }

        public void OnRecvPacket(Session session, ArraySegment<byte> buffer)
        {
            ushort pos = 0;
            ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
            pos += sizeof(ushort);
            ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);

            Func<ArraySegment<byte>, IPacket> func = null;

            if(makePacket.TryGetValue(packetId, out func))
            {
                IPacket packet = func.Invoke(buffer);

                HandlePacket(session, packet);
            }
        }

        
        T MakePacket<T>(ArraySegment<byte> buffer) where T : IPacket, new()
        {
            T packet = new T();
            packet.Read(buffer);
            return packet;
        }

        private void HandlePacket(Session session, IPacket packet)
        {
            Action<Session, IPacket> action = null;
            if (handler.TryGetValue(packet.Protocol, out action))
            {
                action.Invoke(session, packet);
            }
        }
    }
}
