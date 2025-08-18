using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class ErrorCode
    {

        public int Code { get; }
        public string Message { get; }

        private ErrorCode(int code, string message)
        {
            Code = code;
            Message = message;
        }

        //ROOM
        public static readonly ErrorCode FAIL_ROOM_FIND = new ErrorCode(10000, "FAIL_ROOM_FIND");
        public static readonly ErrorCode ALL_PLAYER_NOT_READY = new ErrorCode(100001, " ALL_PLAYER_NOT_READY");
    }
}
