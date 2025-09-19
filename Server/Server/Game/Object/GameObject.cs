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
        public int objectId { get; set; }

        public Map map { get; set; }

        public GameObjectType objectType { get; set; } = GameObjectType.None;

        public GameRoom gameRoom { get; set; }
        public Objectinfo objectinfo { get; set; }

        public virtual void Update() 
        { 
            
        }

        public virtual void OnDamaged(GameObject attacker, int damage) { }
        public virtual void OnDead(GameObject attacker) { }

    }
}
