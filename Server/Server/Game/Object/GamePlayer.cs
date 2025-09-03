using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class GamePlayer : GameObject
    {
        public GamePlayer(ClientSession session, int objectId)
        {
            objectType = GameObjectType.Player;

            Positioninfo pos = new Positioninfo()
            {
                PosX = 0, 
                PosY = 0, 
                PosZ = 0,
            };

            Stateinfo stateInfo = new Stateinfo()
            {
                Hp = 100,
                MaxHp = 100,
                Attack = 10,
                Level = 1,
                Speed = 5,
            };

            objectinfo = new Objectinfo()
            {
                ObjectId = objectId,
                Pos = pos,
                RotY = 0f,
                StateInfo = stateInfo
            };
            this.session = session;
        }

        public ClientSession session;
        public float rotateSpeed = 10f;

        public override void OnDamaged(GameObject attacker, int damage)
        {
            base.OnDamaged(attacker, damage);
        }

        public override void OnDead(GameObject attacker)
        {
            base.OnDead(attacker);
        }
    }
}
