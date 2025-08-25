using Google.Protobuf;
using Google.Protobuf.Protocol;
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

    Dictionary<ushort, Action<Session, ArraySegment<byte>, ushort>> onRecv = new Dictionary<ushort, Action<Session, ArraySegment<byte>, ushort>> ();
    Dictionary<ushort, Action<Session, IMessage>> handler = new Dictionary<ushort, Action<Session, IMessage>>();


    public void Register()
    {
        onRecv.Add((ushort) MsgId.SPong, MakePacket<S_Pong>);
        handler.Add((ushort)MsgId.SPong, PacketHandler.S_PongHandler);

        onRecv.Add((ushort) MsgId.SMovelobby, MakePacket<S_Movelobby>);
        handler.Add((ushort)MsgId.SMovelobby, PacketHandler.S_MovelobbyHandler);

        onRecv.Add((ushort) MsgId.SChat, MakePacket<S_Chat>);
        handler.Add((ushort)MsgId.SChat, PacketHandler.S_ChatHandler);

        onRecv.Add((ushort) MsgId.SReady, MakePacket<S_Ready>);
        handler.Add((ushort)MsgId.SReady, PacketHandler.S_ReadyHandler);

        onRecv.Add((ushort) MsgId.SLoadingstart, MakePacket<S_Loadingstart>);
        handler.Add((ushort)MsgId.SLoadingstart, PacketHandler.S_LoadingstartHandler);

        onRecv.Add((ushort) MsgId.SMove, MakePacket<S_Move>);
        handler.Add((ushort)MsgId.SMove, PacketHandler.S_MoveHandler);

        onRecv.Add((ushort) MsgId.SExitroom, MakePacket<S_Exitroom>);
        handler.Add((ushort)MsgId.SExitroom, PacketHandler.S_ExitroomHandler);

        onRecv.Add((ushort) MsgId.SChangeroominfo, MakePacket<S_Changeroominfo>);
        handler.Add((ushort)MsgId.SChangeroominfo, PacketHandler.S_ChangeroominfoHandler);

        onRecv.Add((ushort) MsgId.SEnterroom, MakePacket<S_Enterroom>);
        handler.Add((ushort)MsgId.SEnterroom, PacketHandler.S_EnterroomHandler);

        onRecv.Add((ushort) MsgId.SRoomlist, MakePacket<S_Roomlist>);
        handler.Add((ushort)MsgId.SRoomlist, PacketHandler.S_RoomlistHandler);

        onRecv.Add((ushort) MsgId.SRoominfo, MakePacket<S_Roominfo>);
        handler.Add((ushort)MsgId.SRoominfo, PacketHandler.S_RoominfoHandler);

        onRecv.Add((ushort) MsgId.SIngamestart, MakePacket<S_Ingamestart>);
        handler.Add((ushort)MsgId.SIngamestart, PacketHandler.S_IngamestartHandler);

        onRecv.Add((ushort) MsgId.SInvite, MakePacket<S_Invite>);
        handler.Add((ushort)MsgId.SInvite, PacketHandler.S_InviteHandler);

        onRecv.Add((ushort) MsgId.SSpawnenemy, MakePacket<S_Spawnenemy>);
        handler.Add((ushort)MsgId.SSpawnenemy, PacketHandler.S_SpawnenemyHandler);

        onRecv.Add((ushort) MsgId.SErrorcode, MakePacket<S_Errorcode>);
        handler.Add((ushort)MsgId.SErrorcode, PacketHandler.S_ErrorcodeHandler);
        
    
    }

    public void OnRecvPacket(Session session, ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);

        Action<Session, ArraySegment<byte>, ushort> action = null;

        if(onRecv.TryGetValue(id, out action))
        {
            action.Invoke(session, buffer, id);
        }
    }


    void MakePacket<T>(Session session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
    {
        T pkt = new T();
        pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);
        Action<Session, IMessage> action = null;
        if(handler.TryGetValue(id, out action))
        {
            action.Invoke(session, pkt);
        }
    }

    public Action<Session, IMessage> GetPacketHandler(ushort id)
    {
        Action<Session, IMessage> action = null;
        if(handler.TryGetValue(id, out action))
        {
            return action;
        }
        return null;
    }
}
