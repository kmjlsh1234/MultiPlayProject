using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class SendBuffer
    {
        public byte[] buffer;
        int usedSize = 0
            ;
        public SendBuffer(int size)
        {
            buffer = new byte[size];
        }

        /// <summary>
        /// 현재 사용가능 공간이 예약공간보다 부족한지 체크
        /// </summary>
        /// <param name="reserveSize"></param>
        /// <returns></returns>
        public bool CheckSenBufferReset(int reserveSize)
        {
            if (reserveSize > buffer.Length - usedSize)
            {
                return true;
            }
            return false;
        }

        public bool CheckCommitAvailable(int commitSize)
        {
            if (commitSize > buffer.Length - usedSize)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// SendBuffer 사용 예약
        /// </summary>
        /// <param name="reserveSize"></param>
        /// <returns></returns>
        public ArraySegment<byte> Reserve(int reserveSize)
        {
            if(reserveSize > buffer.Length - usedSize)
            {
                return null;
            }
            return new ArraySegment<byte>(buffer, usedSize, reserveSize);
        }

        /// <summary>
        /// 진짜 사용 공간 확정
        /// </summary>
        /// <param name="commitSize"></param>
        /// <returns></returns>
        public ArraySegment<byte> Commit(int commitSize)
        {
            if(commitSize > buffer.Length - usedSize)
            {
                return null;
            }
            ArraySegment<byte> commitBuffer = new ArraySegment<byte> (buffer, usedSize, commitSize);
            usedSize += commitSize;
            return commitBuffer;
        }

    }
}
