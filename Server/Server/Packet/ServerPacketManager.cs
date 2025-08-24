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
        
        makePacket.Add((ushort) PacketID.C_PingPacket, MakePacket<C_PingPacket>);
        handler.Add((ushort) PacketID.C_PingPacket, PacketHandler.C_PingPacketHandler);

        makePacket.Add((ushort) PacketID.C_PlayerInfoPacket, MakePacket<C_PlayerInfoPacket>);
        handler.Add((ushort) PacketID.C_PlayerInfoPacket, PacketHandler.C_PlayerInfoPacketHandler);

        makePacket.Add((ushort) PacketID.C_Chat, MakePacket<C_Chat>);
        handler.Add((ushort) PacketID.C_Chat, PacketHandler.C_ChatHandler);

        makePacket.Add((ushort) PacketID.C_ReadyPacket, MakePacket<C_ReadyPacket>);
        handler.Add((ushort) PacketID.C_ReadyPacket, PacketHandler.C_ReadyPacketHandler);

        makePacket.Add((ushort) PacketID.C_StartPacket, MakePacket<C_StartPacket>);
        handler.Add((ushort) PacketID.C_StartPacket, PacketHandler.C_StartPacketHandler);

        makePacket.Add((ushort) PacketID.C_MovePacket, MakePacket<C_MovePacket>);
        handler.Add((ushort) PacketID.C_MovePacket, PacketHandler.C_MovePacketHandler);

        makePacket.Add((ushort) PacketID.C_InputPacket, MakePacket<C_InputPacket>);
        handler.Add((ushort) PacketID.C_InputPacket, PacketHandler.C_InputPacketHandler);

        makePacket.Add((ushort) PacketID.C_ExitRoom, MakePacket<C_ExitRoom>);
        handler.Add((ushort) PacketID.C_ExitRoom, PacketHandler.C_ExitRoomHandler);

        makePacket.Add((ushort) PacketID.C_CreateRoom, MakePacket<C_CreateRoom>);
        handler.Add((ushort) PacketID.C_CreateRoom, PacketHandler.C_CreateRoomHandler);

        makePacket.Add((ushort) PacketID.C_CreateOrJoinRoom, MakePacket<C_CreateOrJoinRoom>);
        handler.Add((ushort) PacketID.C_CreateOrJoinRoom, PacketHandler.C_CreateOrJoinRoomHandler);

        makePacket.Add((ushort) PacketID.C_EnterRoom, MakePacket<C_EnterRoom>);
        handler.Add((ushort) PacketID.C_EnterRoom, PacketHandler.C_EnterRoomHandler);

        makePacket.Add((ushort) PacketID.C_RoomList, MakePacket<C_RoomList>);
        handler.Add((ushort) PacketID.C_RoomList, PacketHandler.C_RoomListHandler);

        makePacket.Add((ushort) PacketID.C_LoadingCompletePacket, MakePacket<C_LoadingCompletePacket>);
        handler.Add((ushort) PacketID.C_LoadingCompletePacket, PacketHandler.C_LoadingCompletePacketHandler);

        makePacket.Add((ushort) PacketID.C_InvitePacket, MakePacket<C_InvitePacket>);
        handler.Add((ushort) PacketID.C_InvitePacket, PacketHandler.C_InvitePacketHandler);

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
