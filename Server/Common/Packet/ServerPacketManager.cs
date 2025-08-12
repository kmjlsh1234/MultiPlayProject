using ServerCore;
using System;
using System.Collections.Generic;

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

    public PacketManager()
    {
        Register();
    }

    Dictionary<ushort, Func<ArraySegment<byte>, IPacket>> makePacket = new Dictionary<ushort, Func<ArraySegment<byte>, IPacket>> ();
    Dictionary<ushort, Action<Session, IPacket>> handler = new Dictionary<ushort, Action<Session, IPacket>>();

    public void Register()
    {
        
        makePacket.Add((ushort) PacketID.C_Chat, MakePacket<C_Chat>);
        handler.Add((ushort) PacketID.C_Chat, PacketHandler.C_ChatHandler);

        makePacket.Add((ushort) PacketID.C_ExitRoom, MakePacket<C_ExitRoom>);
        handler.Add((ushort) PacketID.C_ExitRoom, PacketHandler.C_ExitRoomHandler);

        makePacket.Add((ushort) PacketID.C_EnterRoom, MakePacket<C_EnterRoom>);
        handler.Add((ushort) PacketID.C_EnterRoom, PacketHandler.C_EnterRoomHandler);

        makePacket.Add((ushort) PacketID.TestPacket, MakePacket<TestPacket>);
        handler.Add((ushort) PacketID.TestPacket, PacketHandler.TestPacketHandler);

    }

    public void OnRecvPacket(Session session, ArraySegment<byte> buffer, Action<Session, IPacket> onRecvCallBack = null)
    {
        ushort pos = 0;
        ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);

        Func<ArraySegment<byte>, IPacket> func = null;

        if(makePacket.TryGetValue(packetId, out func))
        {
            IPacket packet = func.Invoke(buffer);
            if(onRecvCallBack != null)
            {
                onRecvCallBack(session, packet);
            }
            else
            {
                HandlePacket(session, packet);
            } 
        }
    }

        
    T MakePacket<T>(ArraySegment<byte> buffer) where T : IPacket, new()
    {
        T packet = new T();
        packet.Read(buffer);
        return packet;
    }

    public void HandlePacket(Session session, IPacket packet)
    {
        Action<Session, IPacket> action = null;
        if (handler.TryGetValue(packet.Protocol, out action))
        {
            action.Invoke(session, packet);
        }
    }





}
