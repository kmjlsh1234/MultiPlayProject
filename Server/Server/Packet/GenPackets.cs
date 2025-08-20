using ServerCore;
using System.Text;
using System;
using System.Collections.Generic;

public enum PacketID
{
    
    C_PingPacket = 0,

    S_PongPacket = 1,

    C_PlayerInfoPacket = 2,

    S_MoveLobbyPacket = 3,

    C_Chat = 4,

    C_ReadyPacket = 5,

    S_BroadCast_ReadyPacket = 6,

    C_StartPacket = 7,

    S_BroadCast_LoadingStartPacket = 8,

    C_MovePacket = 9,

    S_BroadCast_MovePacket = 10,

    S_BroadCast_Chat = 11,

    C_ExitRoom = 12,

    S_BroadCast_ExitRoom = 13,

    S_BroadCast_ChangeRoomInfo = 14,

    C_CreateRoom = 15,

    C_CreateOrJoinRoom = 16,

    C_EnterRoom = 17,

    C_RoomList = 18,

    S_BroadCast_EnterRoom = 19,

    S_RoomInfo = 20,

    S_RoomList = 21,

    S_ErrorCode = 22,

    C_LoadingCompletePacket = 23,

    S_InGameStart = 24,

    C_InvitePacket = 25,

    S_InvitePacket = 26,

}


public class C_PingPacket : IPacket
{
    
    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_PingPacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        

    }

    public ArraySegment<byte> Write()
    {
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
 
                
 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_PongPacket : IPacket
{
    
    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_PongPacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        

    }

    public ArraySegment<byte> Write()
    {
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
 
                
 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class C_PlayerInfoPacket : IPacket
{
    
    public string nickName;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_PlayerInfoPacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        ushort nickNameSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.nickName = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, nickNameSize);
        pos += nickNameSize;


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        ushort nickNameSize = (ushort)Encoding.Unicode.GetByteCount(nickName);

        Array.Copy(
            sourceArray: BitConverter.GetBytes(nickNameSize),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        Array.Copy(
            sourceArray: Encoding.Unicode.GetBytes(this.nickName),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: nickNameSize
        );
        pos += nickNameSize;

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_MoveLobbyPacket : IPacket
{
    
    public int sessionId;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_MoveLobbyPacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.sessionId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.sessionId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class C_Chat : IPacket
{
    
    public string message;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_Chat;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        ushort messageSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.message = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, messageSize);
        pos += messageSize;


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        ushort messageSize = (ushort)Encoding.Unicode.GetByteCount(message);

        Array.Copy(
            sourceArray: BitConverter.GetBytes(messageSize),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        Array.Copy(
            sourceArray: Encoding.Unicode.GetBytes(this.message),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: messageSize
        );
        pos += messageSize;

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class C_ReadyPacket : IPacket
{
    
    public bool isReady;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_ReadyPacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.isReady = BitConverter.ToBoolean(buffer.Array, buffer.Offset + pos);
        pos += sizeof(bool);


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.isReady),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(bool)
        );
        pos += sizeof(bool);

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_BroadCast_ReadyPacket : IPacket
{
    
    public int sessionId;

    public bool isReady;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_BroadCast_ReadyPacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.sessionId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        this.isReady = BitConverter.ToBoolean(buffer.Array, buffer.Offset + pos);
        pos += sizeof(bool);


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.sessionId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.isReady),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(bool)
        );
        pos += sizeof(bool);

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class C_StartPacket : IPacket
{
    
    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_StartPacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        

    }

    public ArraySegment<byte> Write()
    {
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
 
                
 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_BroadCast_LoadingStartPacket : IPacket
{
    
    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_BroadCast_LoadingStartPacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        

    }

    public ArraySegment<byte> Write()
    {
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
 
                
 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class C_MovePacket : IPacket
{
    
    public int playerId;

    public float posX;

    public float posY;

    public float posZ;

    public float rotY;

    public int tick;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_MovePacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.playerId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        this.posX = BitConverter.ToSingle(buffer.Array, buffer.Offset + pos);
        pos += sizeof(float);

        this.posY = BitConverter.ToSingle(buffer.Array, buffer.Offset + pos);
        pos += sizeof(float);

        this.posZ = BitConverter.ToSingle(buffer.Array, buffer.Offset + pos);
        pos += sizeof(float);

        this.rotY = BitConverter.ToSingle(buffer.Array, buffer.Offset + pos);
        pos += sizeof(float);

        this.tick = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.playerId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.posX),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(float)
        );
        pos += sizeof(float);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.posY),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(float)
        );
        pos += sizeof(float);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.posZ),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(float)
        );
        pos += sizeof(float);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.rotY),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(float)
        );
        pos += sizeof(float);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.tick),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_BroadCast_MovePacket : IPacket
{
    
    public int playerId;

    public float posX;

    public float posY;

    public float posZ;

    public float rotY;

    public int tick;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_BroadCast_MovePacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.playerId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        this.posX = BitConverter.ToSingle(buffer.Array, buffer.Offset + pos);
        pos += sizeof(float);

        this.posY = BitConverter.ToSingle(buffer.Array, buffer.Offset + pos);
        pos += sizeof(float);

        this.posZ = BitConverter.ToSingle(buffer.Array, buffer.Offset + pos);
        pos += sizeof(float);

        this.rotY = BitConverter.ToSingle(buffer.Array, buffer.Offset + pos);
        pos += sizeof(float);

        this.tick = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.playerId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.posX),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(float)
        );
        pos += sizeof(float);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.posY),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(float)
        );
        pos += sizeof(float);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.posZ),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(float)
        );
        pos += sizeof(float);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.rotY),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(float)
        );
        pos += sizeof(float);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.tick),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_BroadCast_Chat : IPacket
{
    
    public int sessionId;

    public string message;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_BroadCast_Chat;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.sessionId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        ushort messageSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.message = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, messageSize);
        pos += messageSize;


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.sessionId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        ushort messageSize = (ushort)Encoding.Unicode.GetByteCount(message);

        Array.Copy(
            sourceArray: BitConverter.GetBytes(messageSize),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        Array.Copy(
            sourceArray: Encoding.Unicode.GetBytes(this.message),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: messageSize
        );
        pos += messageSize;

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class C_ExitRoom : IPacket
{
    
    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_ExitRoom;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        

    }

    public ArraySegment<byte> Write()
    {
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
 
                
 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_BroadCast_ExitRoom : IPacket
{
    
    public int sessionId;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_BroadCast_ExitRoom;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.sessionId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.sessionId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_BroadCast_ChangeRoomInfo : IPacket
{
    
    public int roomId;

    public string roomName;

    public int masterId;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_BroadCast_ChangeRoomInfo;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.roomId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        ushort roomNameSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.roomName = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, roomNameSize);
        pos += roomNameSize;

        this.masterId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.roomId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        ushort roomNameSize = (ushort)Encoding.Unicode.GetByteCount(roomName);

        Array.Copy(
            sourceArray: BitConverter.GetBytes(roomNameSize),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        Array.Copy(
            sourceArray: Encoding.Unicode.GetBytes(this.roomName),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: roomNameSize
        );
        pos += roomNameSize;

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.masterId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class C_CreateRoom : IPacket
{
    
    public string roomName;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_CreateRoom;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        ushort roomNameSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.roomName = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, roomNameSize);
        pos += roomNameSize;


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        ushort roomNameSize = (ushort)Encoding.Unicode.GetByteCount(roomName);

        Array.Copy(
            sourceArray: BitConverter.GetBytes(roomNameSize),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        Array.Copy(
            sourceArray: Encoding.Unicode.GetBytes(this.roomName),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: roomNameSize
        );
        pos += roomNameSize;

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class C_CreateOrJoinRoom : IPacket
{
    
    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_CreateOrJoinRoom;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        

    }

    public ArraySegment<byte> Write()
    {
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
 
                
 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class C_EnterRoom : IPacket
{
    
    public int roomId;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_EnterRoom;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.roomId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.roomId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class C_RoomList : IPacket
{
    
    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_RoomList;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        

    }

    public ArraySegment<byte> Write()
    {
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
 
                
 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_BroadCast_EnterRoom : IPacket
{
    
    public int sessionId;

    public string nickName;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_BroadCast_EnterRoom;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.sessionId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        ushort nickNameSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.nickName = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, nickNameSize);
        pos += nickNameSize;


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.sessionId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        ushort nickNameSize = (ushort)Encoding.Unicode.GetByteCount(nickName);

        Array.Copy(
            sourceArray: BitConverter.GetBytes(nickNameSize),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        Array.Copy(
            sourceArray: Encoding.Unicode.GetBytes(this.nickName),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: nickNameSize
        );
        pos += nickNameSize;

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_RoomInfo : IPacket
{
    
    public int roomId;

    public string roomName;

    public int masterId;

    public List<Player> playerList = new List<Player>();

    public class Player
    {
        
    public int sessionId;

    public string nickName;

    public bool isMaster;

    public bool isSelf;

    public bool isReady;


        public void Read(ArraySegment<byte> buffer, ref ushort pos)
        {
                
        this.sessionId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        ushort nickNameSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.nickName = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, nickNameSize);
        pos += nickNameSize;

        this.isMaster = BitConverter.ToBoolean(buffer.Array, buffer.Offset + pos);
        pos += sizeof(bool);

        this.isSelf = BitConverter.ToBoolean(buffer.Array, buffer.Offset + pos);
        pos += sizeof(bool);

        this.isReady = BitConverter.ToBoolean(buffer.Array, buffer.Offset + pos);
        pos += sizeof(bool);
      
        }

        public void Write(ArraySegment<byte> buffer, ref ushort pos)
        {
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.sessionId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        ushort nickNameSize = (ushort)Encoding.Unicode.GetByteCount(nickName);

        Array.Copy(
            sourceArray: BitConverter.GetBytes(nickNameSize),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        Array.Copy(
            sourceArray: Encoding.Unicode.GetBytes(this.nickName),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: nickNameSize
        );
        pos += nickNameSize;

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.isMaster),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(bool)
        );
        pos += sizeof(bool);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.isSelf),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(bool)
        );
        pos += sizeof(bool);

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.isReady),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(bool)
        );
        pos += sizeof(bool);

        }
    }

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_RoomInfo;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.roomId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        ushort roomNameSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.roomName = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, roomNameSize);
        pos += roomNameSize;

        this.masterId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        ushort playerCount = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        for(int i=0; i < playerCount; i++)
        {
            Player player = new Player();
            player.Read(buffer, ref pos);
            playerList.Add(player);
        }


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.roomId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        ushort roomNameSize = (ushort)Encoding.Unicode.GetByteCount(roomName);

        Array.Copy(
            sourceArray: BitConverter.GetBytes(roomNameSize),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        Array.Copy(
            sourceArray: Encoding.Unicode.GetBytes(this.roomName),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: roomNameSize
        );
        pos += roomNameSize;

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.masterId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        ushort playerCount = (ushort) playerList.Count;
            
        Array.Copy(
            sourceArray: BitConverter.GetBytes(playerCount),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        foreach(Player player in playerList)
        {
            player.Write(buffer, ref pos);
        }

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_RoomList : IPacket
{
    
    public List<Room> roomList = new List<Room>();

    public class Room
    {
        
    public int roomId;

    public string roomName;

    public int playerCount;


        public void Read(ArraySegment<byte> buffer, ref ushort pos)
        {
                
        this.roomId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        ushort roomNameSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.roomName = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, roomNameSize);
        pos += roomNameSize;

        this.playerCount = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);
      
        }

        public void Write(ArraySegment<byte> buffer, ref ushort pos)
        {
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.roomId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        ushort roomNameSize = (ushort)Encoding.Unicode.GetByteCount(roomName);

        Array.Copy(
            sourceArray: BitConverter.GetBytes(roomNameSize),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        Array.Copy(
            sourceArray: Encoding.Unicode.GetBytes(this.roomName),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: roomNameSize
        );
        pos += roomNameSize;

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.playerCount),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        }
    }

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_RoomList;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        ushort roomCount = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        for(int i=0; i < roomCount; i++)
        {
            Room room = new Room();
            room.Read(buffer, ref pos);
            roomList.Add(room);
        }


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        ushort roomCount = (ushort) roomList.Count;
            
        Array.Copy(
            sourceArray: BitConverter.GetBytes(roomCount),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        foreach(Room room in roomList)
        {
            room.Write(buffer, ref pos);
        }

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_ErrorCode : IPacket
{
    
    public int code;

    public string message;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_ErrorCode;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.code = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        ushort messageSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.message = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, messageSize);
        pos += messageSize;


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.code),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        ushort messageSize = (ushort)Encoding.Unicode.GetByteCount(message);

        Array.Copy(
            sourceArray: BitConverter.GetBytes(messageSize),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        Array.Copy(
            sourceArray: Encoding.Unicode.GetBytes(this.message),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: messageSize
        );
        pos += messageSize;

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class C_LoadingCompletePacket : IPacket
{
    
    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_LoadingCompletePacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        

    }

    public ArraySegment<byte> Write()
    {
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
 
                
 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_InGameStart : IPacket
{
    
    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_InGameStart;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        

    }

    public ArraySegment<byte> Write()
    {
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
 
                
 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class C_InvitePacket : IPacket
{
    
    public int sessionId;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.C_InvitePacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.sessionId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.sessionId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

public class S_InvitePacket : IPacket
{
    
    public int roomId;

    public string sendUserNickName;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.S_InvitePacket;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
        this.roomId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        ushort sendUserNickNameSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.sendUserNickName = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, sendUserNickNameSize);
        pos += sendUserNickNameSize;


    }

    public ArraySegment<byte> Write()
    {
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
 
                
        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.roomId),
                    sourceIndex: 0,
                    destinationArray: buffer.Array,
                    destinationIndex: buffer.Offset + pos,
                    length: sizeof(int)
        );
        pos += sizeof(int);

        ushort sendUserNickNameSize = (ushort)Encoding.Unicode.GetByteCount(sendUserNickName);

        Array.Copy(
            sourceArray: BitConverter.GetBytes(sendUserNickNameSize),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sizeof(ushort)
        );
        pos += sizeof(ushort);

        Array.Copy(
            sourceArray: Encoding.Unicode.GetBytes(this.sendUserNickName),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset + pos,
            length: sendUserNickNameSize
        );
        pos += sendUserNickNameSize;

 
        Array.Copy(
            sourceArray: BitConverter.GetBytes(pos),
            sourceIndex: 0,
            destinationArray: buffer.Array,
            destinationIndex: buffer.Offset,
            length: sizeof(ushort)
        );

        return SendBufferHelper.Commit(pos);
    }
}

