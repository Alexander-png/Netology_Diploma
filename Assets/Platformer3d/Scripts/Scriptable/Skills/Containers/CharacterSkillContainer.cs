using Platformer3d.Scriptable.Skills.Configurations;
using Platformer3d.SkillSystem.Skills;
using System;
using System.Collections.Generic;
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
            var skill = FindSkill(skillId);
            var modifierDict = new Dictionary<SkillTypes, object>();

            modifierDict[SkillTypes.MaxHealth] = skill.MaxHealth;
            modifierDict[SkillTypes.MaxMana] = skill.MaxMana;
            modifierDict[SkillTypes.DamageImmuneTime] = skill.DamageImmuneTime;

            return new CharacterStatsSkill(skillId, modifierDict);
        }
    }
}