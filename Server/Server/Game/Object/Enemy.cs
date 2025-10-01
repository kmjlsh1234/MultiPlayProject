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

        public void Init(int templateId, Vector2 position, GameRoom gameRoom)
        {
            this.gameRoom = gameRoom;

            objectInfo = new ObjectInfo();
            objectInfo.ObjectId = objectId;
            objectInfo.TemplateId = templateId;
            objectInfo.Pos = new PositionInfo()
            {
                PosX = position.X,
                PosY = 0f,
                PosZ = position.Y
            };
            objectInfo.StateInfo = new StateInfo()
            {
                Hp = 100,
                MaxHp = 100,
                Attack = 10,
                Level = 1,
                Speed = 2,
            };
            objectInfo.CellInfo = gameRoom.map.WorldToCell(objectInfo.Pos);
            
            targetPlayer = FindTargetPlayer();
            if(targetPlayer != null)
            {
                objectInfo.TargetId = targetPlayer.objectId;
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
                int distance = gameRoom.map.GetCellDistance(objectInfo.CellInfo, player.objectInfo.CellInfo);
                if (distance < minDintance)
                {
                    minDintance = distance;
                    target = player;
                }
            }

            objectInfo.TargetId = target.objectId;
            return target;
        }

        public void UpdateGridPosition()
        {
            CellInfo newCell = gameRoom.map.WorldToCell(objectInfo.Pos);

            if (newCell != objectInfo.CellInfo)
            {
                gameRoom.map.UpdateObjectPosition(this, objectInfo.CellInfo, newCell);
            }
            objectInfo.CellInfo = newCell;
        }

        public void UpdateEnemyPos()
        {
            
        }
    }
}
