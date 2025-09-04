using Google.Protobuf;
using Server.Game.Job;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Server
{
    public abstract class Room<T> : JobSerializer
    {
        public int roomId { get; set; }
        public int masterId { get; set; }

        public abstract void BroadCast(IMessage packet);

        public abstract void EnterRoom(T t);

        public abstract void ExitRoom(T t);

    }
}
