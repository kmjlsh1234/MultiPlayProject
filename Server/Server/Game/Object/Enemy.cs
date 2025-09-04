using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Object
{
    public class Enemy : GameObject
    {
        public GamePlayer targetPlayer;

        public Enemy()
        {
            objectType = GameObjectType.Enemy;

            Positioninfo pos = new Positioninfo()
            {
                PosX = 72,
                PosY = 0,
                PosZ = 72,
            };

            Stateinfo stateInfo = new Stateinfo()
            {
                Hp = 100,
                MaxHp = 100,
                Attack = 10,
                Level = 1,
                Speed = 2,
            };

            objectinfo = new Objectinfo()
            {
                ObjectId = objectId,
                Pos = pos,
                RotY = 0f,
                StateInfo = stateInfo
            };
        }
    }
}
