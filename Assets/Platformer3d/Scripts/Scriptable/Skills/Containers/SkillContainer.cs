using Platformer3d.SkillSystem.Skills;
using UnityEngine;

namespace Platformer3d.Scriptable.Skills.Containers
{
	public abstract class SkillContainer<T> : ScriptableObject where T : Skill
    {
        public abstract T CreateSkill(string skillId);
    }
}