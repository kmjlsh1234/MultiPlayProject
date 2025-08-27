using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class Player
    {
        public GameRoom room {  get; set; }
        public ClientSession session { get; set; }

        public PlayerInfo playerInfo { get; set; } = new PlayerInfo();
    }
}
