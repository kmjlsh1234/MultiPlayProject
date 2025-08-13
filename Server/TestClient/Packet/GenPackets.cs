using ServerCore;
using System.Text;
using System;
using System.Collections.Generic;

public enum PacketID
{
    
    C_Chat = 0,

    S_BroadCast_Chat = 1,

    C_ExitRoom = 2,

    S_BroadCast_ExitRoom = 3,

    C_CreateRoom = 4,

    C_EnterRoom = 5,

    S_BroadCast_EnterRoom = 6,

    TestPacket = 7,

    S_PlayerList = 8,

    C_RoomList = 9,

    S_RoomList = 10,

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

public class S_BroadCast_EnterRoom : IPacket
{
    
    public int sessionId;

    
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

public class TestPacket : IPacket
{
    
    public int playerId;

    public string message;

    
    public ushort Protocol
    {
        get
        {
            return (ushort) PacketID.TestPacket;
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
                    sourceArray: BitConverter.GetBytes(this.playerId),
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

public class S_PlayerList : IPacket
{
    
    public List<Player> playerList = new List<Player>();

    public class Player
    {
        
    public int sessionId;

    public bool isSelf;


        public void Read(ArraySegment<byte> buffer, ref ushort pos)
        {
                
        this.sessionId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        this.isSelf = BitConverter.ToBoolean(buffer.Array, buffer.Offset + pos);
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

        Array.Copy(
                    sourceArray: BitConverter.GetBytes(this.isSelf),
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
            return (ushort) PacketID.S_PlayerList;
        }
    }
    
    public void Read(ArraySegment<byte> buffer)
    {
        ushort pos = 0;
        ushort pacetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);

        
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

public class S_RoomList : IPacket
{
    
    public List<Room> roomList = new List<Room>();

    public class Room
    {
        
    public int roomId;

    public string roomName;


        public void Read(ArraySegment<byte> buffer, ref ushort pos)
        {
                
        this.roomId = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
        pos += sizeof(int);

        ushort roomNameSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
        pos += sizeof(ushort);
        this.roomName = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + pos, roomNameSize);
        pos += roomNameSize;
      
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

