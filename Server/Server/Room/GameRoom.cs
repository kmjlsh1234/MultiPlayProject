using Google.Protobuf;
using Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoom : Room<GamePlayer>
    {
        public Dictionary<int, GamePlayer> players = new Dictionary<int, GamePlayer>();

        #region :::: Abstract Function
        public override void BroadCast(IMessage message)
        {
            
        }

        public override void EnterRoom(GamePlayer player)
        {
            lock (key)
            {
                players.Add(player.session.sessionId, player);

                //나에게 정보 전송

                //우리팀에게 브로드캐스트
            }
        }

        public override void ExitRoom(GamePlayer player)
        {
            
        }
        #endregion


    }
}
