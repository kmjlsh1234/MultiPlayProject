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
        onRecv.Add((ushort) MsgId.SPongcheck, MakePacket<S_Pongcheck>);
        handler.Add((ushort)MsgId.SPongcheck, PacketHandler.S_PongcheckHandler);

        onRecv.Add((ushort) MsgId.SConnect, MakePacket<S_Connect>);
        handler.Add((ushort)MsgId.SConnect, PacketHandler.S_ConnectHandler);

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

        onRecv.Add((ushort) MsgId.SEntermatchroom, MakePacket<S_Entermatchroom>);
        handler.Add((ushort)MsgId.SEntermatchroom, PacketHandler.S_EntermatchroomHandler);

        onRecv.Add((ushort) MsgId.SMatchroominfo, MakePacket<S_Matchroominfo>);
        handler.Add((ushort)MsgId.SMatchroominfo, PacketHandler.S_MatchroominfoHandler);

        onRecv.Add((ushort) MsgId.SIngamestart, MakePacket<S_Ingamestart>);
        handler.Add((ushort)MsgId.SIngamestart, PacketHandler.S_IngamestartHandler);

        onRecv.Add((ushort) MsgId.SSpawnenemy, MakePacket<S_Spawnenemy>);
        handler.Add((ushort)MsgId.SSpawnenemy, PacketHandler.S_SpawnenemyHandler);

        onRecv.Add((ushort) MsgId.SErrorcode, MakePacket<S_Errorcode>);
        handler.Add((ushort)MsgId.SErrorcode, PacketHandler.S_ErrorcodeHandler);

        onRecv.Add((ushort) MsgId.SExitgameroom, MakePacket<S_Exitgameroom>);
        handler.Add((ushort)MsgId.SExitgameroom, PacketHandler.S_ExitgameroomHandler);

        onRecv.Add((ushort) MsgId.SEnemymove, MakePacket<S_Enemymove>);
        handler.Add((ushort)MsgId.SEnemymove, PacketHandler.S_EnemymoveHandler);

        onRecv.Add((ushort) MsgId.SGameroominfo, MakePacket<S_Gameroominfo>);
        handler.Add((ushort)MsgId.SGameroominfo, PacketHandler.S_GameroominfoHandler);

        onRecv.Add((ushort) MsgId.SEnterpartyroom, MakePacket<S_Enterpartyroom>);
        handler.Add((ushort)MsgId.SEnterpartyroom, PacketHandler.S_EnterpartyroomHandler);

        onRecv.Add((ushort) MsgId.SPartyroominfo, MakePacket<S_Partyroominfo>);
        handler.Add((ushort)MsgId.SPartyroominfo, PacketHandler.S_PartyroominfoHandler);

        onRecv.Add((ushort) MsgId.SExitpartyroom, MakePacket<S_Exitpartyroom>);
        handler.Add((ushort)MsgId.SExitpartyroom, PacketHandler.S_ExitpartyroomHandler);

        onRecv.Add((ushort) MsgId.SInvite, MakePacket<S_Invite>);
        handler.Add((ushort)MsgId.SInvite, PacketHandler.S_InviteHandler);

        onRecv.Add((ushort) MsgId.SMatchstart, MakePacket<S_Matchstart>);
        handler.Add((ushort)MsgId.SMatchstart, PacketHandler.S_MatchstartHandler);

        onRecv.Add((ushort) MsgId.SInput, MakePacket<S_Input>);
        handler.Add((ushort)MsgId.SInput, PacketHandler.S_InputHandler);

        onRecv.Add((ushort) MsgId.SExp, MakePacket<S_Exp>);
        handler.Add((ushort)MsgId.SExp, PacketHandler.S_ExpHandler);

        onRecv.Add((ushort) MsgId.SLevelup, MakePacket<S_Levelup>);
        handler.Add((ushort)MsgId.SLevelup, PacketHandler.S_LevelupHandler);

        onRecv.Add((ushort) MsgId.SSkillselect, MakePacket<S_Skillselect>);
        handler.Add((ushort)MsgId.SSkillselect, PacketHandler.S_SkillselectHandler);

        onRecv.Add((ushort) MsgId.SLevelupfinish, MakePacket<S_Levelupfinish>);
        handler.Add((ushort)MsgId.SLevelupfinish, PacketHandler.S_LevelupfinishHandler);
        
    
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
