using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketGenerator
{
    // {0} : 등록
    public class PacketFormat
    {

        public static string managerFormat =
@"using ServerCore;
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

    Dictionary<ushort, Func<ArraySegment<byte>, IPacket>> makePacket = new Dictionary<ushort, Func<ArraySegment<byte>, IPacket>> ();
    Dictionary<ushort, Action<Session, IPacket>> handler = new Dictionary<ushort, Action<Session, IPacket>>();

    public void Register()
    {{
        {0}
    }}

    public void OnRecvPacket(Session session, ArraySegment<byte> buffer, Action<Session, IPacket> onRecvCallBack = null)
    {{
        ushort pos = 0;
        ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);

        Func<ArraySegment<byte>, IPacket> func = null;

        if(makePacket.TryGetValue(packetId, out func))
        {{
            IPacket packet = func.Invoke(buffer);
            if(onRecvCallBack != null)
            {{
                onRecvCallBack(session, packet);
            }}
            else
            {{
                HandlePacket(session, packet);
            }} 
        }}
    }}

        
    T MakePacket<T>(ArraySegment<byte> buffer) where T : IPacket, new()
    {{
        T packet = new T();
        packet.Read(buffer);
        return packet;
    }}

    public void HandlePacket(Session session, IPacket packet)
    {{
        Action<Session, IPacket> action = null;
        if (handler.TryGetValue(packet.Protocol, out action))
        {{
            action.Invoke(session, packet);
        }}
    }}





}}
";

        // {0} : 패킷 명
        public static string managerRegistFormat =
@"
        makePacket.Add((ushort) PacketID.{0}, MakePacket<{0}>);
        handler.Add((ushort) PacketID.{0}, PacketHandler.{0}Handler);
";

        // {0} : 패킷 ID ENUM
        // {1} : 패킷 클래스 모음
        public static string fileFormat =
@"using ServerCore;
using System.Text;
using System;
using System.Collections.Generic;

public enum PacketID
{{
    {0}
}}

{1}
";

        // {0} : 클래스 이름
        // {1} : 멤버 변수들
        // {2} : Read
        // {3} : Write
        public static string packetFormat =
@"
public class {0} : IPacket
{{
    {1}
    
    public ushort Protocol
    {{
        get
        {{
            return (ushort) PacketID.{0};
        }}
    }}
    
    public void Read(ArraySegment<byte> buffer)
    {{
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        {2}

    }}

    public ArraySegment<byte> Write()
    {{
        ArraySegment<byte> buffer = SendBufferHelper.Reserve(4096);

        ushort pos = 0;

        //패킷 사이즈 공간만큼 더하기
        pos += sizeof(ushort);

        //버퍼에 패킷 ID 추가
        Array.Copy(
            sourceArray: BitConverter.GetBytes(Protocol),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
            );
        //패킷 id만큼 사이즈 추가
        pos += sizeof(ushort);
 
                {3}
 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }}
}}
";

        // {0} : 변수 타입
        // {1} : 변수 명
        public static string memberFormat = 
@"
    public {0} {1};
"
;

        // {0} : 변수 명 대무자
        // {1} : 변수 명 소문자
        // {2} : 리스트 멤버변수
        // {3} : 리스트 Read
        // {4} : 리스트 Write
        public static string memberListFormat =
@"
    public List<{0}> {1}List = new List<{0}>();

    public class {0}
    {{
        {2}

        public void Read(ArraySegment<byte> buffer, ref ushort pos)
        {{
                {3}      
        }}

        public void Write(ArraySegment<byte> buffer, ref ushort pos)
        {{
                {4}
        }}
    }}
";

        // {0} : 변수 명 대무자
        // {1} : 변수 명 소문자
        public static string memberListReadFormat =
@"
        ushort {1}Count = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        for(int i=0; i < {1}Count; i++)
        {{
            {0} {1} = new {0}();
            {1}.Read(buffer, ref pos);
            {1}List.Add({1});
        }}
";

        // {0} : 변수 명 대무자
        // {1} : 변수 명 소문자
        public static string memberListWriteFormat =
@"
        ushort {1}Count = (ushort) {1}List.Count;
            
        Array.Copy(
            sourceArray: BitConverter.GetBytes({1}Count),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        foreach({0} {1} in {1}List)
        {{
            {1}.Write(buffer, ref pos);
        }}
";


        // {0} : 변수 명
        // {1} : 변환 함수
        // {2} 변수 형식
        public static string readFormat =
@"
        this.{0} = BitConverter.{1}(buffer.Array, buffer.Offset + pos);
        pos += sizeof({2});
";

        // {0} : 변수 명
        // {1} : 변수 타입
        public static string writeFormat =
@"
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.{0}),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof({1})
        );
        pos += sizeof({1});
";

        // {0} 변수 명
        public static string readStringFormat =
@"
        ushort {0}Size = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.{0} = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, {0}Size);
        pos += {0}Size;
";

        // {0} 변수 명
        public static string writeStringFormat =
@"
        ushort {0}Size = (ushort)Encoding.Unicode.GetByteCount({0});

        Array.Copy(
            sourceArray: BitConverter.GetBytes({0}Size),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        Array.Copy(
            sourceArray: Encoding.Unicode.GetBytes(this.{0}),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: {0}Size
        );
        pos += {0}Size;
";

        //{0} : 패킷 명
        //{1} : 패킷 ID
        public static string packetEnumFormat = 
@"
    {0} = {1},
";
    }
}
