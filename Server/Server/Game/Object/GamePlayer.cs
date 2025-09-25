using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class GamePlayer : GameObject
    {
        public ClientSession session;
        public bool isSkillSelect = false;
        public Vector3 currentCellPos;

        //Skill
        public SkillManageComponent skillManageComponent;

        public GamePlayer(ClientSession session, GameRoom room)
        {
            this.session = session;
            this.gameRoom = room;
            skillManageComponent = new SkillManageComponent(this);


            objectId = session.sessionId;
            objectType = GameObjectType.Player;
            
            Positioninfo pos = new Positioninfo()
            {
                PosX = 0, 
                PosY = 0, 
                PosZ = 0,
            };

            Stateinfo stateInfo = new Stateinfo()
            {
                Hp = 600,
                MaxHp = 600,
                Attack = 10,
                Level = 1,
                Speed = 3,
            };

            objectinfo = new Objectinfo()
            {
                ObjectId = objectId,
                Pos = pos,
                StateInfo = stateInfo,
                CellInfo = gameRoom.map.WorldToCell(pos)
            };
        }

        public override void OnDamaged(GameObject attacker, int damage)
        {
            base.OnDamaged(attacker, damage);
        }

        public override void OnDead(GameObject attacker)
        {
            base.OnDead(attacker);
        }

        public override void Update()
        {
            base.Update();
            //UpdateGridPosition();
        }

        public void UpdateGridPosition()
        {
            Cellinfo newCell = gameRoom.map.WorldToCell(objectinfo.Pos);

            if(newCell != objectinfo.CellInfo)
            {
                gameRoom.map.UpdateObjectPosition(this, objectinfo.CellInfo, newCell);
            }
            objectinfo.CellInfo = newCell;
        }
    }
}
