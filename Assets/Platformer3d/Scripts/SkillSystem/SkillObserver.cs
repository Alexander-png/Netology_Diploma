using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.Movement.Base;
using Platformer3d.GameCore;
using Platformer3d.SkillSystem.Skills;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Platformer3d.SkillSystem
{
	public class SkillObserver : MonoBehaviour, ISaveable
	{
        [Inject]
        private GameSystem _gameSystem;

        [SerializeField]
        private CharacterMovement _movementController;
        [SerializeField]
        private Character _character;

        [SerializeField]
        private bool _distinctSkillsOnly = true;

        private List<Skill> _appliedSkills = new List<Skill>();

        private Skill FindSkill(string id) => _appliedSkills.Find(s => s.SkillId == id);

        private class Skilldata : SaveData
        {
            public List<Skill> AppliedSkills;
        }

        private void Start()
        {
            _gameSystem.RegisterSaveableObject(this);
        }

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

        private bool ValidateData(Skilldata data)
        {
            if (data == null)
            {
                EditorExtentions.GameLogger.AddMessage($"Failed to cast data. Instance name: {gameObject.name}, data type: {data}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            if (data.Name != gameObject.name)
            {
                EditorExtentions.GameLogger.AddMessage($"Attempted to set data from another game object. Instance name: {gameObject.name}, data name: {data.Name}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            return true;
        }

        public object GetData() => new Skilldata
        {
            Name = gameObject.name,
            AppliedSkills = new List<Skill>(_appliedSkills)
        };

        public void SetData(object data)
        {
            Skilldata dataToSet = data as Skilldata;
            if (!ValidateData(dataToSet))
            {
                return;
            }

            _appliedSkills.ForEach(s => RemoveSkill(s));
            _appliedSkills = new List<Skill>(_appliedSkills);
            _appliedSkills.ForEach(s => AddSkill(s));
        }
    }
}