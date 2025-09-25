using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class SkillManageComponent
    {
        public GamePlayer owner;
        public Dictionary<int, Skillinfo> activeSkills = new Dictionary<int, Skillinfo>();
        
        public SkillManageComponent(GamePlayer owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// 새로운 스킬 추가
        /// </summary>
        /// <param name="info"></param>
        public void AddSkill(Skillinfo info)
        {
            activeSkills.Add(info.Id, info);
        }

        /// <summary>
        /// 특정 스킬을 찾아 레벨업
        /// </summary>
        /// <param name="id"></param>
        public void UpgradeSkill(int id)
        {
            if(activeSkills.TryGetValue(id, out Skillinfo info))
            {
                info.SkillLevel++;
            }
        }

        public List<Skillinfo> GetSelectList()
        {
            return null;
        }
    }
}
