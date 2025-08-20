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
            
        }
    }
}
