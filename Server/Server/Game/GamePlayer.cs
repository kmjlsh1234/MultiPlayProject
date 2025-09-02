using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class GamePlayer
    {
        public ClientSession session;
        public Vector3 position;
        public float rotY = 0f;
        public float moveSpeed = 5f;
        public float rotateSpeed = 10f;
    }
}
