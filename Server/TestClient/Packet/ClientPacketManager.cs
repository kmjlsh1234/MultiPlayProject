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
        
        makePacket.Add((ushort) PacketID.S_PongPacket, MakePacket<S_PongPacket>);
        handler.Add((ushort) PacketID.S_PongPacket, PacketHandler.S_PongPacketHandler);

        makePacket.Add((ushort) PacketID.S_MoveLobbyPacket, MakePacket<S_MoveLobbyPacket>);
        handler.Add((ushort) PacketID.S_MoveLobbyPacket, PacketHandler.S_MoveLobbyPacketHandler);

        makePacket.Add((ushort) PacketID.S_BroadCast_ReadyPacket, MakePacket<S_BroadCast_ReadyPacket>);
        handler.Add((ushort) PacketID.S_BroadCast_ReadyPacket, PacketHandler.S_BroadCast_ReadyPacketHandler);

        makePacket.Add((ushort) PacketID.S_BroadCast_LoadingStartPacket, MakePacket<S_BroadCast_LoadingStartPacket>);
        handler.Add((ushort) PacketID.S_BroadCast_LoadingStartPacket, PacketHandler.S_BroadCast_LoadingStartPacketHandler);

        makePacket.Add((ushort) PacketID.S_BroadCast_MovePacket, MakePacket<S_BroadCast_MovePacket>);
        handler.Add((ushort) PacketID.S_BroadCast_MovePacket, PacketHandler.S_BroadCast_MovePacketHandler);

        makePacket.Add((ushort) PacketID.S_BroadCast_Chat, MakePacket<S_BroadCast_Chat>);
        handler.Add((ushort) PacketID.S_BroadCast_Chat, PacketHandler.S_BroadCast_ChatHandler);

        makePacket.Add((ushort) PacketID.S_BroadCast_ExitRoom, MakePacket<S_BroadCast_ExitRoom>);
        handler.Add((ushort) PacketID.S_BroadCast_ExitRoom, PacketHandler.S_BroadCast_ExitRoomHandler);

        makePacket.Add((ushort) PacketID.S_BroadCast_ChangeRoomInfo, MakePacket<S_BroadCast_ChangeRoomInfo>);
        handler.Add((ushort) PacketID.S_BroadCast_ChangeRoomInfo, PacketHandler.S_BroadCast_ChangeRoomInfoHandler);

        makePacket.Add((ushort) PacketID.S_BroadCast_EnterRoom, MakePacket<S_BroadCast_EnterRoom>);
        handler.Add((ushort) PacketID.S_BroadCast_EnterRoom, PacketHandler.S_BroadCast_EnterRoomHandler);

        makePacket.Add((ushort) PacketID.S_RoomInfo, MakePacket<S_RoomInfo>);
        handler.Add((ushort) PacketID.S_RoomInfo, PacketHandler.S_RoomInfoHandler);

        makePacket.Add((ushort) PacketID.S_RoomList, MakePacket<S_RoomList>);
        handler.Add((ushort) PacketID.S_RoomList, PacketHandler.S_RoomListHandler);

        makePacket.Add((ushort) PacketID.S_ErrorCode, MakePacket<S_ErrorCode>);
        handler.Add((ushort) PacketID.S_ErrorCode, PacketHandler.S_ErrorCodeHandler);

        makePacket.Add((ushort) PacketID.S_InGameStart, MakePacket<S_InGameStart>);
        handler.Add((ushort) PacketID.S_InGameStart, PacketHandler.S_InGameStartHandler);

        makePacket.Add((ushort) PacketID.S_InvitePacket, MakePacket<S_InvitePacket>);
        handler.Add((ushort) PacketID.S_InvitePacket, PacketHandler.S_InvitePacketHandler);

        makePacket.Add((ushort) PacketID.S_BroadCast_SpawnEnemy, MakePacket<S_BroadCast_SpawnEnemy>);
        handler.Add((ushort) PacketID.S_BroadCast_SpawnEnemy, PacketHandler.S_BroadCast_SpawnEnemyHandler);

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
