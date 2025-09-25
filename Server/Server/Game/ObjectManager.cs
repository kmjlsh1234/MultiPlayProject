using Google.Protobuf.Protocol;
using Server.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class ObjectManager : SingletonBase<ObjectManager>
    {
        object key = new object();

        Dictionary<int, GamePlayer> players = new Dictionary<int, GamePlayer>();

        int objectId = 0;

        public T Add<T>(Vector2 position) where T : GameObject
        {
            System.Type type = typeof(T);

            lock (key)
            {
                objectId++;

                if (type.Equals(typeof(GamePlayer)))
                {
                    return null;
                }
                else if(type.Equals(typeof(Enemy)))
                {
                    Enemy enemy = new Enemy();
                    enemy.objectId = objectId;                  

                    return enemy as T;
                }

                return null;
            }
        }

        
    }
}
