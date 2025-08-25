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
        onRecv.Add((ushort) MsgId.CPing, MakePacket<C_Ping>);
        handler.Add((ushort)MsgId.CPing, PacketHandler.C_PingHandler);

        onRecv.Add((ushort) MsgId.CPlayerinfo, MakePacket<C_Playerinfo>);
        handler.Add((ushort)MsgId.CPlayerinfo, PacketHandler.C_PlayerinfoHandler);

        onRecv.Add((ushort) MsgId.CChat, MakePacket<C_Chat>);
        handler.Add((ushort)MsgId.CChat, PacketHandler.C_ChatHandler);

        onRecv.Add((ushort) MsgId.CReady, MakePacket<C_Ready>);
        handler.Add((ushort)MsgId.CReady, PacketHandler.C_ReadyHandler);

        onRecv.Add((ushort) MsgId.CStart, MakePacket<C_Start>);
        handler.Add((ushort)MsgId.CStart, PacketHandler.C_StartHandler);

        onRecv.Add((ushort) MsgId.CMove, MakePacket<C_Move>);
        handler.Add((ushort)MsgId.CMove, PacketHandler.C_MoveHandler);

        onRecv.Add((ushort) MsgId.CInput, MakePacket<C_Input>);
        handler.Add((ushort)MsgId.CInput, PacketHandler.C_InputHandler);

        onRecv.Add((ushort) MsgId.CExitroom, MakePacket<C_Exitroom>);
        handler.Add((ushort)MsgId.CExitroom, PacketHandler.C_ExitroomHandler);

        onRecv.Add((ushort) MsgId.CCreateroom, MakePacket<C_Createroom>);
        handler.Add((ushort)MsgId.CCreateroom, PacketHandler.C_CreateroomHandler);

        onRecv.Add((ushort) MsgId.CCreateorjoinroom, MakePacket<C_Createorjoinroom>);
        handler.Add((ushort)MsgId.CCreateorjoinroom, PacketHandler.C_CreateorjoinroomHandler);

        onRecv.Add((ushort) MsgId.CEnterroom, MakePacket<C_Enterroom>);
        handler.Add((ushort)MsgId.CEnterroom, PacketHandler.C_EnterroomHandler);

        onRecv.Add((ushort) MsgId.CRoomlist, MakePacket<C_Roomlist>);
        handler.Add((ushort)MsgId.CRoomlist, PacketHandler.C_RoomlistHandler);

        onRecv.Add((ushort) MsgId.CLoadingcomplete, MakePacket<C_Loadingcomplete>);
        handler.Add((ushort)MsgId.CLoadingcomplete, PacketHandler.C_LoadingcompleteHandler);

        onRecv.Add((ushort) MsgId.CInvite, MakePacket<C_Invite>);
        handler.Add((ushort)MsgId.CInvite, PacketHandler.C_InviteHandler);
        
    
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
