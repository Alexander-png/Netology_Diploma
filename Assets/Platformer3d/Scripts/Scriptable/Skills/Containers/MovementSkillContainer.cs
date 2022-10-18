using Platformer3d.Scriptable.Skills.Configurations;
using Platformer3d.SkillSystem.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer3d.Scriptable.Skills.Containers
{
    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Containers/Movement skill container")]
    public class MovementSkillContainer : SkillContainer<CharacterMovementSkill>
    {
        [SerializeField]
        private MovementSkillConfiguration[] _skills;

        private MovementSkillConfiguration FindSkill(string id) =>
            Array.Find(_skills, x => x.SkillId == id);

        public override CharacterMovementSkill CreateSkill(string skillId)
        {
            var skill = FindSkill(skillId);
            var modifierDict = new Dictionary<SkillTypes, object>();

            modifierDict[SkillTypes.MaxSpeed] = skill.MaxSpeed;
            modifierDict[SkillTypes.Accleration] = skill.Acceleration;
            modifierDict[SkillTypes.JumpCountInRow] = skill.JumpCountInRow;
            modifierDict[SkillTypes.ClimbForce] = skill.ClimbForce;
            modifierDict[SkillTypes.WallClimbRepulsion] = skill.DashDistance;
            modifierDict[SkillTypes.DashDistance] = skill.DashDistance;

            return new CharacterMovementSkill(skillId, modifierDict);
        }
    }
}