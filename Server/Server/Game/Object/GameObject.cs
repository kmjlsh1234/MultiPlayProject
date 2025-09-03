using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class GameObject
    {
        GameObjectType objectType { get; set; } = GameObjectType.None;

        public GameRoom gameRoom { get; set; }


    }
}
