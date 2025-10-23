using Google.Protobuf.Protocol;
using Server.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class SpawningPool
    {
        public GameRoom gameRoom;
        public int maxEnemyCount = 100;

        public Random rand = new Random();
        public List<Vector2> spawnPoints = new List<Vector2>();
        

        public SpawningPool(GameRoom gameRoom)
        {
            this.gameRoom = gameRoom;

            for (int x = -25; x < 26; x++) {
                Vector2 point1 = new Vector2();
                point1.X = x;
                point1.Y = 25;
                spawnPoints.Add(point1);

                Vector2 point2 = new Vector2();
                point2.X = x;
                point2.Y = -25;
                spawnPoints.Add(point2);
            }

            for (int y = -25; y < 26; y++)
            {
                Vector2 point1 = new Vector2();
                point1.X = 25;
                point1.Y = y;
                spawnPoints.Add(point1);

                Vector2 point2 = new Vector2();
                point2.X = -25;
                point2.Y = y;
                spawnPoints.Add(point2);
            }
        }

        public Enemy TrySpawn()
        {
            int monsterCount = gameRoom.enemies.Count;

            if(monsterCount >= maxEnemyCount)
            {
                return null;
            }

            
            int i = rand.Next(spawnPoints.Count);
            Vector2 point = spawnPoints[i];

            Enemy enemy = ObjectManager.Instance.Add<Enemy>(point);

            int templateId = 200004; //rand.Next(0, 2) == 0 ? 200002 : 200004;
            //templateId = rand.Next(0, 3) == 0 ? templateId : 200001;
            
            Console.WriteLine($"Spawn {templateId}");
            enemy.Init(templateId, point, gameRoom);

            return enemy;
        }
    }
}
