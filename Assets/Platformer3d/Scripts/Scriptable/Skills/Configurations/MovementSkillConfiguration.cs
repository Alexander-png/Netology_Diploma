using Platformer3d.SkillSystem.Skills;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer3d.Scriptable.Skills.Configurations
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Movement skill")]
	public class MovementSkillConfiguration : SkillConfiguration
	{
		[SerializeField]
		private float _maxSpeed;
		[SerializeField]
		private float _acceleration;
		[SerializeField]
		private float _jumpCountInRow;
		[SerializeField]
		private float _climbForce;
		[SerializeField]
		private float _wallClimbRepulsion;
		[SerializeField]
		private float _dashDistance;
        [SerializeField]
		private float _dashDuration;
		[SerializeField]
		private float _dashRechargeTime;

        public override Dictionary<SkillTypes, object> GetSkills()
        {
			var skillDict = new Dictionary<SkillTypes, object>();
			skillDict[SkillTypes.MaxSpeed] = _maxSpeed;
			skillDict[SkillTypes.Accleration] = _acceleration;
			skillDict[SkillTypes.JumpCountInRow] = _jumpCountInRow;
			skillDict[SkillTypes.ClimbForce] = _climbForce;
			skillDict[SkillTypes.WallClimbRepulsion] = _wallClimbRepulsion;
			skillDict[SkillTypes.DashDistance] = _dashDistance;
			skillDict[SkillTypes.DashDuration] = _dashDuration;
			skillDict[SkillTypes.DashRechargeTime] = _dashRechargeTime;
			return skillDict;
		}
    }
}