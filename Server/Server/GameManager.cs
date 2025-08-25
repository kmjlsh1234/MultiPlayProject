using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameManager
    {
        private Room room;
        private Timer gameTimer;
        private int timer = 0;
        private Random random = new Random();
        public void StartGame(Room room)
        {
            this.room = room;
            gameTimer = new Timer(
                callback: (e) => GameTick(),
                state: null,
                dueTime: 1000,
                period: 1000
                );
        }

        void GameTick()
        {
            timer++;
            Console.WriteLine($"Timer : {timer}");
            if (timer % 10 == 0)
            {
                /*
                S_BroadCast_SpawnEnemy packet = new S_BroadCast_SpawnEnemy()
                {
                    posX = random.Next(0, 2) == 0 ? -50 : 50,
                    posZ = random.Next(0, 2) == 0 ? -50 : 50,
                };

                room.BroadCast(packet.Write());
                Console.WriteLine($"Spawn Enemy at ({packet.posX}, {packet.posZ})");
                */
            }
        }
    }
}
