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
        onRecv.Add((ushort) MsgId.CPingCheck, MakePacket<C_PingCheck>);
        handler.Add((ushort)MsgId.CPingCheck, PacketHandler.C_PingCheckHandler);

        onRecv.Add((ushort) MsgId.CConnect, MakePacket<C_Connect>);
        handler.Add((ushort)MsgId.CConnect, PacketHandler.C_ConnectHandler);

        onRecv.Add((ushort) MsgId.CChat, MakePacket<C_Chat>);
        handler.Add((ushort)MsgId.CChat, PacketHandler.C_ChatHandler);

        onRecv.Add((ushort) MsgId.CReady, MakePacket<C_Ready>);
        handler.Add((ushort)MsgId.CReady, PacketHandler.C_ReadyHandler);

        onRecv.Add((ushort) MsgId.CStart, MakePacket<C_Start>);
        handler.Add((ushort)MsgId.CStart, PacketHandler.C_StartHandler);

        onRecv.Add((ushort) MsgId.CMove, MakePacket<C_Move>);
        handler.Add((ushort)MsgId.CMove, PacketHandler.C_MoveHandler);

        onRecv.Add((ushort) MsgId.CExitRoom, MakePacket<C_ExitRoom>);
        handler.Add((ushort)MsgId.CExitRoom, PacketHandler.C_ExitRoomHandler);

        onRecv.Add((ushort) MsgId.CCreateRoom, MakePacket<C_CreateRoom>);
        handler.Add((ushort)MsgId.CCreateRoom, PacketHandler.C_CreateRoomHandler);

        onRecv.Add((ushort) MsgId.CCreateOrJoinRoom, MakePacket<C_CreateOrJoinRoom>);
        handler.Add((ushort)MsgId.CCreateOrJoinRoom, PacketHandler.C_CreateOrJoinRoomHandler);

        onRecv.Add((ushort) MsgId.CEnterMatchRoom, MakePacket<C_EnterMatchRoom>);
        handler.Add((ushort)MsgId.CEnterMatchRoom, PacketHandler.C_EnterMatchRoomHandler);

        onRecv.Add((ushort) MsgId.CLoadingComplete, MakePacket<C_LoadingComplete>);
        handler.Add((ushort)MsgId.CLoadingComplete, PacketHandler.C_LoadingCompleteHandler);

        onRecv.Add((ushort) MsgId.CExitGameRoom, MakePacket<C_ExitGameRoom>);
        handler.Add((ushort)MsgId.CExitGameRoom, PacketHandler.C_ExitGameRoomHandler);

        onRecv.Add((ushort) MsgId.CEnemyMove, MakePacket<C_EnemyMove>);
        handler.Add((ushort)MsgId.CEnemyMove, PacketHandler.C_EnemyMoveHandler);

        onRecv.Add((ushort) MsgId.CCreatePartyRoom, MakePacket<C_CreatePartyRoom>);
        handler.Add((ushort)MsgId.CCreatePartyRoom, PacketHandler.C_CreatePartyRoomHandler);

        onRecv.Add((ushort) MsgId.CEnterPartyRoom, MakePacket<C_EnterPartyRoom>);
        handler.Add((ushort)MsgId.CEnterPartyRoom, PacketHandler.C_EnterPartyRoomHandler);

        onRecv.Add((ushort) MsgId.CExitPartyRoom, MakePacket<C_ExitPartyRoom>);
        handler.Add((ushort)MsgId.CExitPartyRoom, PacketHandler.C_ExitPartyRoomHandler);

        onRecv.Add((ushort) MsgId.CInvite, MakePacket<C_Invite>);
        handler.Add((ushort)MsgId.CInvite, PacketHandler.C_InviteHandler);

        onRecv.Add((ushort) MsgId.CInput, MakePacket<C_Input>);
        handler.Add((ushort)MsgId.CInput, PacketHandler.C_InputHandler);

        onRecv.Add((ushort) MsgId.CExp, MakePacket<C_Exp>);
        handler.Add((ushort)MsgId.CExp, PacketHandler.C_ExpHandler);

        onRecv.Add((ushort) MsgId.CNewSkill, MakePacket<C_NewSkill>);
        handler.Add((ushort)MsgId.CNewSkill, PacketHandler.C_NewSkillHandler);

        onRecv.Add((ushort) MsgId.CUpgradeSkill, MakePacket<C_UpgradeSkill>);
        handler.Add((ushort)MsgId.CUpgradeSkill, PacketHandler.C_UpgradeSkillHandler);
        
    
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
