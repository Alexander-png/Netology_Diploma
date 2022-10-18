using UnityEngine;

namespace Platformer3d.Scriptable.Skills.Configurations
{
	public abstract class SkillConfiguration : ScriptableObject
	{
		[SerializeField]
		private string _skillId;

		public string SkillId => _skillId;
	}
}