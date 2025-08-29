using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class PlayerManager
    {
        public static PlayerManager Instance { get; } = new PlayerManager();
        object key = new object();

        Dictionary<int, Player> players = new Dictionary<int, Player>();
        int playerId = 1;   //TODO

        public Player Add(ClientSession session)
        {
            
            lock (key)
            {
                Player player = new Player();
                player.session = session;
                players.Add(playerId, player);
                playerId++;
                return player;
            }
        }
       
        public bool Remove(int playerId)
        {
            lock (key)
            {
                return players.Remove(playerId);
            }
        }

        public Player Find(int playerId)
        {
            lock (key)
            {
                Player player = null;
                if (players.TryGetValue(playerId, out player))
                {
                    return player;
                }
                return null;
            }
        }
    }
}
