using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{
    static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }

    public PacketManager()
    {
        Register();
    }

    Dictionary<ushort, Action<Session, ArraySegment<byte>, ushort>> onRecv = new Dictionary<ushort, Action<Session, ArraySegment<byte>, ushort>> ();
    Dictionary<ushort, Action<Session, IMessage>> handler = new Dictionary<ushort, Action<Session, IMessage>>();

    public Action<Session, IMessage, ushort> CustomHandler { get; set; }

    public void Register()
    {
        onRecv.Add((ushort) MsgId.CPingcheck, MakePacket<C_Pingcheck>);
        handler.Add((ushort)MsgId.CPingcheck, PacketHandler.C_PingcheckHandler);

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

        onRecv.Add((ushort) MsgId.CExitroom, MakePacket<C_Exitroom>);
        handler.Add((ushort)MsgId.CExitroom, PacketHandler.C_ExitroomHandler);

        onRecv.Add((ushort) MsgId.CCreateroom, MakePacket<C_Createroom>);
        handler.Add((ushort)MsgId.CCreateroom, PacketHandler.C_CreateroomHandler);

        onRecv.Add((ushort) MsgId.CCreateorjoinroom, MakePacket<C_Createorjoinroom>);
        handler.Add((ushort)MsgId.CCreateorjoinroom, PacketHandler.C_CreateorjoinroomHandler);

        onRecv.Add((ushort) MsgId.CEntermatchroom, MakePacket<C_Entermatchroom>);
        handler.Add((ushort)MsgId.CEntermatchroom, PacketHandler.C_EntermatchroomHandler);

        onRecv.Add((ushort) MsgId.CRoomlist, MakePacket<C_Roomlist>);
        handler.Add((ushort)MsgId.CRoomlist, PacketHandler.C_RoomlistHandler);

        onRecv.Add((ushort) MsgId.CLoadingcomplete, MakePacket<C_Loadingcomplete>);
        handler.Add((ushort)MsgId.CLoadingcomplete, PacketHandler.C_LoadingcompleteHandler);

        onRecv.Add((ushort) MsgId.CInvite, MakePacket<C_Invite>);
        handler.Add((ushort)MsgId.CInvite, PacketHandler.C_InviteHandler);

        onRecv.Add((ushort) MsgId.CExitgameroom, MakePacket<C_Exitgameroom>);
        handler.Add((ushort)MsgId.CExitgameroom, PacketHandler.C_ExitgameroomHandler);

        onRecv.Add((ushort) MsgId.CEnemymove, MakePacket<C_Enemymove>);
        handler.Add((ushort)MsgId.CEnemymove, PacketHandler.C_EnemymoveHandler);
        
    
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

        if(CustomHandler != null)
        {
            CustomHandler.Invoke(session, pkt, id);
        }
        else
        {
            Action<Session, IMessage> action = null;
            if (handler.TryGetValue(id, out action))
            {
                action.Invoke(session, pkt);
            }
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
