using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class RecvBuffer
    {
        public byte[] buffer;

        int readPos=0;
        int writePos=0;

        public RecvBuffer(int size)
        {
            buffer = new byte[size];
        }

        public void Clean()
        {
            //데이터가 남아있을 경우
            if(writePos - readPos > 0)
            {
                int dataSize = writePos - readPos;
                Array.Copy(
                    sourceArray: new ArraySegment<byte>(buffer, readPos, dataSize).Array,
                    sourceIndex: readPos,
                    destinationArray: buffer,
                    destinationIndex: 0,
                    length: dataSize
                    );

                readPos = 0;
                writePos = dataSize;
            }
            else
            {
                readPos = 0;
                writePos = 0;
            }
        }

        public ArraySegment<byte> getAvailableArea()
        {
            return new ArraySegment<byte>(buffer, writePos, buffer.Length - writePos);
        }

        public ArraySegment<byte> getReadArea()
        {
            return new ArraySegment<byte>(buffer, readPos, writePos - readPos);
        }

        public bool OnWrite(int numOfBytes)
        {
            if(buffer.Length - writePos < numOfBytes)
            {
                return false;
            }
            writePos += numOfBytes;
            return true;
        }

        public bool OnRead(int numOfBytes)
        {
            if(writePos - readPos < numOfBytes)
            {
                return false;
            }
            readPos += numOfBytes;
            return true;
        }
    }
}
