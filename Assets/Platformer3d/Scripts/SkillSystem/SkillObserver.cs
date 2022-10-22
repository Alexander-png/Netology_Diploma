using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.Movement.Base;
using Platformer3d.SkillSystem.Skills;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer3d.SkillSystem
{
	public class SkillObserver : MonoBehaviour
	{
        [SerializeField]
        private CharacterMovement _movementController;
        [SerializeField]
        private Character _character;

        [SerializeField]
        private bool _distinctSkillsOnly = true;

        private List<Skill> _appliedSkills = new List<Skill>();

        private Skill FindSkill(string id) => _appliedSkills.Find(s => s.SkillId == id);

        public void AddSkill(Skill skill)
        {
            if (_distinctSkillsOnly)
            {
                if (FindSkill(skill.SkillId) != null)
                {
                    return;
                }
            }

            AddSkillToEntity(skill);
            _appliedSkills.Add(skill);
        }

		public void RemoveSkill(Skill skill)
        {
            Skill skillToRemove = FindSkill(skill.SkillId);
            if (skillToRemove != null)
            {
                RemoveSkillFromEntity(skillToRemove);
                _appliedSkills.Remove(skillToRemove);
            }
        }

        private void AddSkillToEntity(Skill skill)
        {
            if (skill is CharacterStatsSkill stats)
            {
                throw new System.NotImplementedException();
            }
            else if (skill is CharacterMovementSkill moves)
            {
                _movementController.AddStats(moves.GetData());
            }
        }

        private void RemoveSkillFromEntity(Skill skill)
        {
            if (skill is CharacterStatsSkill stats)
            {
                throw new System.NotImplementedException();
            }
            else if (skill is CharacterMovementSkill moves)
            {
                _movementController.RemoveStats(moves.GetData());
            }
        }
	}
}