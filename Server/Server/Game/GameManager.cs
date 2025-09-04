using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class GameManager
    {
        public GameRoom room { get; set; }

        public GameManager(GameRoom room)
        {
            this.room = room;
        }

        public void SpawnEnemy()
        {

        }
    }
}
