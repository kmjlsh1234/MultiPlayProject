using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public interface IPacket
    {
        ushort Protocol { get; }
        void Read(ArraySegment<byte> buffer);
        ArraySegment<byte> Write();
    }
}
