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
        public static readonly ErrorCode SESSION_NOT_FOUND = new ErrorCode(100002, "SESSION_NOT_FOUND");
        public static readonly ErrorCode SESSION_ALREADY_IN_ROOM = new ErrorCode(100003, "SESSION_ALREADY_IN_ROOM");
        public static readonly ErrorCode MAX_ROOM_COUNT = new ErrorCode(100004, "MAX_ROOM_COUNT");
    }
}
