using Platformer3d.SkillSystem.Skills;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer3d.Scriptable.Skills.Configurations
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Character skill")]
	public class CharacterSkillConfiguration : SkillConfiguration
	{
		[SerializeField]
		private float _maxHealth;
		[SerializeField]
		private float _maxMana;
		[SerializeField]
		private float _damageImmuneTime;

		public float MaxHealth => _maxHealth;
		public float MaxMana => _maxMana;
		public float DamageImmuneTime => _damageImmuneTime;

        public override Dictionary<SkillTypes, object> GetSkills()
        {
			var skillDict = new Dictionary<SkillTypes, object>();
			skillDict[SkillTypes.MaxHealth] = MaxHealth;
			skillDict[SkillTypes.MaxMana] = MaxMana;
			skillDict[SkillTypes.DamageImmuneTime] = DamageImmuneTime;
			return skillDict;
		}
    }
}