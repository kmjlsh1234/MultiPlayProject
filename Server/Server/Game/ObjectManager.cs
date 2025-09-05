using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class ObjectManager : SingletonBase<ObjectManager>
    {
        object key = new object();

        Dictionary<int, GamePlayer> players = new Dictionary<int, GamePlayer>();

        int objectId = 0;

        public T Add<T>() where T : GameObject, new()
        {
            T go = new T();
            lock (key)
            {
                go.objectId = objectId;
                objectId++;
                if(go.objectType == GameObjectType.Player)
                {
                    players.Add(go.objectId, go as GamePlayer);
                }
            }

            return go;
        }

        
    }
}
