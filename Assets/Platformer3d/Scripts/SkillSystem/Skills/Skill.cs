using Platformer3d.CharacterSystem.Movement.Base;
using System;
using System.Collections.Generic;

namespace Platformer3d.SkillSystem.Skills
{
    public enum SkillTypes : byte
    {
        MaxHealth = 0,
        MaxMana = 1,
        DamageImmuneTime = 2,
        MaxSpeed = 3,
        Accleration = 4,
        JumpCountInRow = 5,
        ClimbForce = 6,
        WallClimbRepulsion = 7,

        DashDistance = 8,
        DashDuration = 9,
        DashRechargeTime = 10,
    }

    /// <summary>
    /// Skill modificator, that applied to character directly
    /// </summary>
	public abstract class Skill
    {
        protected string _skillId;
        protected Dictionary<SkillTypes, object> _skills;

        public string SkillId => _skillId;
        public Dictionary<SkillTypes, object> Skills => _skills;
    }

    public class CharacterStatsSkill : Skill
    {
        public CharacterStatsSkill(string id, Dictionary<SkillTypes, object> skillsDict)
        {
            _skillId = id;
            _skills = skillsDict;
        }

        public CharacterStatsInfo GetData()
        {
            throw new System.NotImplementedException();
        }
    }

    public class CharacterMovementSkill : Skill
    {
        public CharacterMovementSkill(string id, Dictionary<SkillTypes, object> skillsDict)
        {
            _skillId = id;
            _skills = skillsDict;
        }

        public MovementStatsInfo GetData() => new MovementStatsInfo()
        {
            Acceleration = Convert.ToSingle(_skills[SkillTypes.Accleration]),
            MaxSpeed = Convert.ToSingle(_skills[SkillTypes.MaxSpeed]),
            JumpCountInRow = Convert.ToInt32(_skills[SkillTypes.JumpCountInRow]),
            ClimbForce = Convert.ToSingle(_skills[SkillTypes.ClimbForce]),
            WallClimbRepulsion = Convert.ToSingle(_skills[SkillTypes.WallClimbRepulsion]),
            DashForce = Convert.ToSingle(_skills[SkillTypes.DashDistance]),
            DashDuration = Convert.ToSingle(_skills[SkillTypes.DashDuration]),
            DashRechargeTime = Convert.ToSingle(_skills[SkillTypes.DashRechargeTime]),
        };
    }
}