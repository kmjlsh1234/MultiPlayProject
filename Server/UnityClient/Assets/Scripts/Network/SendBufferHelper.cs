using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    /// <summary>
    /// 각 스레드마다 큰 버퍼를 들고 있게 하기
    /// 거기서 필요한 만큼 잘라서 송신에 쓰기
    /// </summary>
    public class SendBufferHelper
    {
        public static ThreadLocal<SendBuffer> CurrentBuffer = new ThreadLocal<SendBuffer>();
        public static int ChunkSize = 65000 * 100;

        public static ArraySegment<byte> Reserve(int reserveSize)
        {
            //SendBuffer 없으면 새로 생성
            if(CurrentBuffer.Value == null)
            {
                CurrentBuffer.Value = new SendBuffer(ChunkSize);
            }


            //현재 사용가능 공간이 예약공간보다 부족하면 Reset
            if (CurrentBuffer.Value.CheckSenBufferReset(reserveSize))
            {
                CurrentBuffer.Value = new SendBuffer(ChunkSize);
            }

            return CurrentBuffer.Value.Reserve(reserveSize);
        } 

        public static ArraySegment<byte> Commit(int commitSize)
        {
            return CurrentBuffer.Value.Commit(commitSize);
        }
    }
}
