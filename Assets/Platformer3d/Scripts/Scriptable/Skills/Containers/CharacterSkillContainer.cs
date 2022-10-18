using Platformer3d.Scriptable.Skills.Configurations;
using Platformer3d.SkillSystem.Skills;
using System;
using UnityEngine;

namespace Platformer3d.Scriptable.Skills.Containers
{
    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Containers/Character skill container")]
    public class CharacterSkillContainer : SkillContainer<CharacterStatsSkill>
    {
        [SerializeField]
        private CharacterSkillConfiguration[] _skills;

        private CharacterSkillConfiguration FindSkill(string id) =>
            Array.Find(_skills, x => x.SkillId == id);

        public override CharacterStatsSkill CreateSkill(string skillId)
        {
            return new CharacterStatsSkill(skillId, FindSkill(skillId).GetSkills());
        }
    }
}