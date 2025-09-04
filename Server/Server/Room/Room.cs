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
    public enum RoomType
    {
        None = 0,
        Match = 1,
        Game = 2,
    }

    public abstract class Room : JobSerializer
    {
        public int roomId { get; set; }
        public int masterId { get; set; }

        public RoomType roomType { get; set; } = RoomType.None;

        public abstract void BroadCast(IMessage packet);

        public abstract void EnterRoom(ClientSession session);

        public abstract void ExitRoom(ClientSession session);

        public abstract void Update();

    }
}
