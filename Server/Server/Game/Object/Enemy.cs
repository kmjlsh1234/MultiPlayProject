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
        
        public Random rand = new Random();
        float[,] spawnPoints = new float[,]
        { 
            { 5f, 5f },
            { 5f, -5f },
            { -5f, 5f },
            { -5f, -5f },
        };

        public GamePlayer target;

        public Enemy()
        {
            
            objectType = GameObjectType.Enemy;
            
            int i = rand.Next(spawnPoints.GetLength(0));

            Positioninfo pos = new Positioninfo()
            {
                PosX = spawnPoints[i, 0],
                PosY = 0.1f,
                PosZ = spawnPoints[i, 1],
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
                Pos = pos,
                StateInfo = stateInfo
            };
        }
        public override void Update()
        {
            base.Update();
        }
    }

}
