using Newtonsoft.Json.Linq;
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

        private void Start() =>
            _gameSystem.RegisterSaveableObject(this);

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

        private bool ValidateData(JObject data)
        {
            if (data == null)
            {
                EditorExtentions.GameLogger.AddMessage($"Failed to cast data. Instance name: {gameObject.name}, data type: {data}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            if (data.Value<string>("Name") != gameObject.name)
            {
                EditorExtentions.GameLogger.AddMessage($"Attempted to set data from another game object. Instance name: {gameObject.name}, data name: {data.Value<string>("Name")}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            return true;
        }

        public JObject GetData()
        {
            var data = new JObject();
            data["Name"] = gameObject.name;
            data["AppliedSkills"] = JToken.FromObject(new List<Skill>(_appliedSkills));   
            return data;
        }

        public bool SetData(JObject data)
        {
            if (!ValidateData(data))
            {
                return false;
            }
            _appliedSkills.ForEach(s => RemoveSkill(s));
            _appliedSkills = new List<Skill>(data.Value<List<Skill>>("AppliedSkills"));
            _appliedSkills.ForEach(s => AddSkill(s));
            return true;
        }
    }
}