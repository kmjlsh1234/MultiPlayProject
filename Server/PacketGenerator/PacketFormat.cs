using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketGenerator
{
    
    public class PacketFormat
    {
        // {0} : 등록
        public static string managerFormat =
@"using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{{
    #region
    private static PacketManager instance;
    public static PacketManager Instance
    {{
        get
        {{
            if(instance == null)
            {{
                instance = new PacketManager();
            }}
            return instance;
        }}
    }}
    #endregion

    public PacketManager()
    {{
        Register();
    }}

    Dictionary<ushort, Action<Session, ArraySegment<byte>, ushort>> onRecv = new Dictionary<ushort, Action<Session, ArraySegment<byte>, ushort>> ();
    Dictionary<ushort, Action<Session, IMessage>> handler = new Dictionary<ushort, Action<Session, IMessage>>();


    public void Register()
    {{{0}        
    
    }}

    public void OnRecvPacket(Session session, ArraySegment<byte> buffer)
    {{
        ushort pos = 0;
        ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);

        Action<Session, ArraySegment<byte>, ushort> action = null;

        if(onRecv.TryGetValue(id, out action))
        {{
            action.Invoke(session, buffer, id);
        }}
    }}


    void MakePacket<T>(Session session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
    {{
        T pkt = new T();
        pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);
        Action<Session, IMessage> action = null;
        if(handler.TryGetValue(id, out action))
        {{
            action.Invoke(session, pkt);
        }}
    }}

    public Action<Session, IMessage> GetPacketHandler(ushort id)
    {{
        Action<Session, IMessage> action = null;
        if(handler.TryGetValue(id, out action))
        {{
            return action;
        }}
        return null;
    }}
}}
";


        // {0} : MsgId
        // {1} : 패킷이름
        public static string managerRegisterFormat =
@"
        onRecv.Add((ushort) MsgId.{0}, MakePacket<{1}>);
        handler.Add((ushort)MsgId.{0}, PacketHandler.{1}Handler);
";
    }
}
