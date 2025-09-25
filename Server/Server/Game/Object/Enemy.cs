using Google.Protobuf.Protocol;
using Google.Protobuf.WellKnownTypes;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Object
{
    public class Enemy : GameObject
    {
        public GamePlayer targetPlayer;

        public Enemy()
        {
            objectType = GameObjectType.Enemy;
        }

        public void Init(Vector2 position, GameRoom gameRoom)
        {
            this.gameRoom = gameRoom;

            objectinfo = new Objectinfo();
            objectinfo.ObjectId = objectId;
            objectinfo.TemplateId = 200001;
            objectinfo.Pos = new Positioninfo()
            {
                PosX = position.X,
                PosY = 0f,
                PosZ = position.Y
            };
            objectinfo.StateInfo = new Stateinfo()
            {
                Hp = 100,
                MaxHp = 100,
                Attack = 10,
                Level = 1,
                Speed = 2,
            };
            objectinfo.CellInfo = gameRoom.map.WorldToCell(objectinfo.Pos);
            
            targetPlayer = FindTargetPlayer();
            if(targetPlayer != null)
            {
                objectinfo.TargetId = targetPlayer.objectId;
            }
            
        }

        public override void Update()
        {
            UpdateGridPosition();
            FindTargetPlayer();
        }

        public override void OnDamaged(GameObject attacker, int damage)
        {
            base.OnDamaged(attacker, damage);
        }

        public override void OnDead(GameObject attacker)
        {
            base.OnDead(attacker);
            gameRoom.map.Remove(this);
        }

        public GamePlayer FindTargetPlayer()
        {

            GamePlayer target = null;
            int minDintance = int.MaxValue;
            foreach (GamePlayer player in gameRoom.players.Values)
            {
                int distance = gameRoom.map.GetCellDistance(objectinfo.CellInfo, player.objectinfo.CellInfo);
                if (distance < minDintance)
                {
                    minDintance = distance;
                    target = player;
                }
            }

            objectinfo.TargetId = target.objectId;
            return target;
        }

        public void UpdateGridPosition()
        {
            Cellinfo newCell = gameRoom.map.WorldToCell(objectinfo.Pos);

            if (newCell != objectinfo.CellInfo)
            {
                gameRoom.map.UpdateObjectPosition(this, objectinfo.CellInfo, newCell);
            }
            objectinfo.CellInfo = newCell;
        }

        public void UpdateEnemyPos()
        {
            
        }
    }
}
