using Platformer3d.CharacterSystem.DataContainers;
using Platformer3d.CharacterSystem.Enums;
using Platformer3d.EditorExtentions;
using Platformer3d.Scriptable.Characters;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Base
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField]
        private DefaultCharacterStats _stats;

        public SideTypes Side { get; protected set; }
        public string Name { get; protected set; }

        protected virtual void Awake() 
        {
            if (_stats == null)
            {
                GameLogger.AddMessage($"{nameof(Character)} ({gameObject.name}): no stats assigned.", GameLogger.LogType.Fatal);
            }
            SetDefaultParameters(_stats);
        }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }

        protected virtual void SetDefaultParameters(DefaultCharacterStats stats)
        {
            Name = stats.Name;
            Side = stats.Side;
        }

        public virtual void OnRespawn() { }

        public virtual void SetDataFromContainer(CharacterDataContainer data)
        {
            Name = data.Name;
            Side = data.Side;
            transform.position = data.Position;
        }

        public virtual CharacterDataContainer GetDataAsContainer() => new CharacterDataContainer()
        {
            Side = Side,
            Name = Name,
            Position = transform.position,
        };
    }
}
