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
    public enum RoomState
    {
        None,
        LobbySate,
        MatchState,
        GameState,
    }

    public enum RoomType
    {
        None = 0,
        Party = 1,
        Match = 2,
        Game = 3,
    }

    public abstract class Room : JobSerializer
    {
        public int roomId { get; set; }
        public int masterId { get; set; }

        public int maxCount { get; set; }

        public virtual int PlayerCount { get; }

        protected object key = new object();

        public RoomState roomState { get; set; } = RoomState.None;

        public RoomType roomType { get; set; } = RoomType.None;

        public abstract void BroadCast(IMessage packet);

        public abstract void EnterRoom(ClientSession session);

        public abstract void ExitRoom(ClientSession session);

        public abstract void Update();

    }
}
