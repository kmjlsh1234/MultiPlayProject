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

        public ArraySegment<byte> AvailableWriteArea
        {
            get
            {
                return new ArraySegment<byte>(buffer, readPos, buffer.Length - writePos);
            }
        }

        public RecvBuffer(int size)
        {
            buffer = new byte[size];
        }

        public void Clean()
        {

        }

        
        public void OnRead()
        {

        }

        public void OnWrite()
        {

        }

    }
}
