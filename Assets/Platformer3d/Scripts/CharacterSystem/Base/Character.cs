using Platformer3d.CharacterSystem.Enums;
using Platformer3d.EditorExtentions;
using Platformer3d.Scriptable;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Base
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField]
        private string _name;
        [SerializeField]
        private CharacterStats _stats;

        public CharacterStats Stats => _stats;
        public SideTypes Side => _stats.Side;
        public string Name => _name;

        protected virtual void Awake() 
        {
            if (_stats == null)
            {
                GameLogger.AddMessage($"{nameof(Character)} ({gameObject.name}): no stats assigned.", GameLogger.LogType.Fatal);
            }
            FillStatsFields();
        }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }

        protected virtual void FillStatsFields()
        {

        }
    }
}
