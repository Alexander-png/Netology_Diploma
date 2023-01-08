using Newtonsoft.Json.Linq;
using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.Movement.Base;
using Platformer3d.GameCore;
using Platformer3d.SkillSystem.Skills;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Platformer3d.SkillSystem
{
	public class SkillObserver : MonoBehaviour, ISaveable
	{
        private const string SaveableEntityId = "player_Skills";

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
            public List<string> AppliedSkillIds;
        }

        private void Start()
        {
            _gameSystem.RegisterSaveableObject(this);
        }

        public void AddSkill(string skillId)
        {
            var skill = _gameSystem.PlayerMovementSkillContainer.CreateSkill(skillId);
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

        public bool CheckSkillAdded(string skillId) =>
            FindSkill(skillId) != null;

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
                EditorExtentions.GameLogger.AddMessage($"Failed to cast data. Instance name: {SaveableEntityId}, data type: {data}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            if (data.Name != SaveableEntityId)
            {
                EditorExtentions.GameLogger.AddMessage($"Attempted to set data from another game object. Instance name: {SaveableEntityId}, data name: {data.Name}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            return true;
        }

        public object GetData() => new Skilldata
        {
            Name = SaveableEntityId,
            AppliedSkillIds = new List<string>(_appliedSkills.Select(s => s.SkillId).ToList()),
        };

        public bool SetData(object data)
        {
            Skilldata dataToSet = data as Skilldata;
            if (!ValidateData(dataToSet))
            {
                return false;
            }

            _appliedSkills.ForEach(s => RemoveSkill(s));
            _appliedSkills = new List<Skill>();
            dataToSet.AppliedSkillIds.ForEach(skillId => AddSkill(skillId));
            return true;
        }

        public bool SetData(JObject data) => 
            SetData(data.ToObject<Skilldata>());
    }
}