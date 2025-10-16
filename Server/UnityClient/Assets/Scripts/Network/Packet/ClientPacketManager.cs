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
        onRecv.Add((ushort) MsgId.SPongCheck, MakePacket<S_PongCheck>);
        handler.Add((ushort)MsgId.SPongCheck, PacketHandler.S_PongCheckHandler);

        onRecv.Add((ushort) MsgId.SConnect, MakePacket<S_Connect>);
        handler.Add((ushort)MsgId.SConnect, PacketHandler.S_ConnectHandler);

        onRecv.Add((ushort) MsgId.SChat, MakePacket<S_Chat>);
        handler.Add((ushort)MsgId.SChat, PacketHandler.S_ChatHandler);

        onRecv.Add((ushort) MsgId.SReady, MakePacket<S_Ready>);
        handler.Add((ushort)MsgId.SReady, PacketHandler.S_ReadyHandler);

        onRecv.Add((ushort) MsgId.SLoadingStart, MakePacket<S_LoadingStart>);
        handler.Add((ushort)MsgId.SLoadingStart, PacketHandler.S_LoadingStartHandler);

        onRecv.Add((ushort) MsgId.SMove, MakePacket<S_Move>);
        handler.Add((ushort)MsgId.SMove, PacketHandler.S_MoveHandler);

        onRecv.Add((ushort) MsgId.SExitRoom, MakePacket<S_ExitRoom>);
        handler.Add((ushort)MsgId.SExitRoom, PacketHandler.S_ExitRoomHandler);

        onRecv.Add((ushort) MsgId.SEnterMatchRoom, MakePacket<S_EnterMatchRoom>);
        handler.Add((ushort)MsgId.SEnterMatchRoom, PacketHandler.S_EnterMatchRoomHandler);

        onRecv.Add((ushort) MsgId.SMatchRoomInfo, MakePacket<S_MatchRoomInfo>);
        handler.Add((ushort)MsgId.SMatchRoomInfo, PacketHandler.S_MatchRoomInfoHandler);

        onRecv.Add((ushort) MsgId.SInGameStart, MakePacket<S_InGameStart>);
        handler.Add((ushort)MsgId.SInGameStart, PacketHandler.S_InGameStartHandler);

        onRecv.Add((ushort) MsgId.SSpawnEnemy, MakePacket<S_SpawnEnemy>);
        handler.Add((ushort)MsgId.SSpawnEnemy, PacketHandler.S_SpawnEnemyHandler);

        onRecv.Add((ushort) MsgId.SErrorCode, MakePacket<S_ErrorCode>);
        handler.Add((ushort)MsgId.SErrorCode, PacketHandler.S_ErrorCodeHandler);

        onRecv.Add((ushort) MsgId.SExitGameRoom, MakePacket<S_ExitGameRoom>);
        handler.Add((ushort)MsgId.SExitGameRoom, PacketHandler.S_ExitGameRoomHandler);

        onRecv.Add((ushort) MsgId.SEnemyMove, MakePacket<S_EnemyMove>);
        handler.Add((ushort)MsgId.SEnemyMove, PacketHandler.S_EnemyMoveHandler);

        onRecv.Add((ushort) MsgId.SGameRoomInfo, MakePacket<S_GameRoomInfo>);
        handler.Add((ushort)MsgId.SGameRoomInfo, PacketHandler.S_GameRoomInfoHandler);

        onRecv.Add((ushort) MsgId.SEnterPartyRoom, MakePacket<S_EnterPartyRoom>);
        handler.Add((ushort)MsgId.SEnterPartyRoom, PacketHandler.S_EnterPartyRoomHandler);

        onRecv.Add((ushort) MsgId.SPartyRoomInfo, MakePacket<S_PartyRoomInfo>);
        handler.Add((ushort)MsgId.SPartyRoomInfo, PacketHandler.S_PartyRoomInfoHandler);

        onRecv.Add((ushort) MsgId.SExitPartyRoom, MakePacket<S_ExitPartyRoom>);
        handler.Add((ushort)MsgId.SExitPartyRoom, PacketHandler.S_ExitPartyRoomHandler);

        onRecv.Add((ushort) MsgId.SInvite, MakePacket<S_Invite>);
        handler.Add((ushort)MsgId.SInvite, PacketHandler.S_InviteHandler);

        onRecv.Add((ushort) MsgId.SMatchStart, MakePacket<S_MatchStart>);
        handler.Add((ushort)MsgId.SMatchStart, PacketHandler.S_MatchStartHandler);

        onRecv.Add((ushort) MsgId.SInput, MakePacket<S_Input>);
        handler.Add((ushort)MsgId.SInput, PacketHandler.S_InputHandler);

        onRecv.Add((ushort) MsgId.SExp, MakePacket<S_Exp>);
        handler.Add((ushort)MsgId.SExp, PacketHandler.S_ExpHandler);

        onRecv.Add((ushort) MsgId.SLevelUp, MakePacket<S_LevelUp>);
        handler.Add((ushort)MsgId.SLevelUp, PacketHandler.S_LevelUpHandler);

        onRecv.Add((ushort) MsgId.SSkillSelect, MakePacket<S_SkillSelect>);
        handler.Add((ushort)MsgId.SSkillSelect, PacketHandler.S_SkillSelectHandler);

        onRecv.Add((ushort) MsgId.SLevelUpFinish, MakePacket<S_LevelUpFinish>);
        handler.Add((ushort)MsgId.SLevelUpFinish, PacketHandler.S_LevelUpFinishHandler);

        onRecv.Add((ushort) MsgId.SSkill, MakePacket<S_Skill>);
        handler.Add((ushort)MsgId.SSkill, PacketHandler.S_SkillHandler);
        
    
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
