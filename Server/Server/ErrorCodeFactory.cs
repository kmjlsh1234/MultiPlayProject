using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ErrorCodeFactory
    {
        public static S_Errorcode GetErrorCode(ErrorCode errorCode)
        {
            return new S_Errorcode()
            {
                Code = errorCode.Code,
                Message = errorCode.Message,
            };
        }
    }
}
