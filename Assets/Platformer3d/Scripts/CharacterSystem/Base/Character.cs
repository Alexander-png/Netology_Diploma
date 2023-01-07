using Newtonsoft.Json;
using Platformer3d.CharacterSystem.DataContainers;
using Platformer3d.CharacterSystem.Enums;
using Platformer3d.EditorExtentions;
using Platformer3d.GameCore;
using Platformer3d.Scriptable.Characters;
using System;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Base
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField]
        private DefaultCharacterStats _stats;
        [SerializeField]
        protected Transform _visual;

        public event EventHandler Respawning;

        protected class CharacterData : SaveData
        {
            public struct Position3
            {
                public float x;
                public float y;
                public float z;
            }
            public Position3 RawPosition;
            public SideTypes Side;
            public float CurrentHealth;

            public Vector3 GetPositionAsVector3() => new Vector3(RawPosition.x, RawPosition.y, RawPosition.z);
        }

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

        public virtual void NotifyRespawn() => Respawning?.Invoke(this, EventArgs.Empty); 

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

        protected virtual bool ValidateData(CharacterData data)
        {
            if (data == null)
            {
                GameLogger.AddMessage($"Failed to cast data. Instance name: {gameObject.name}, data type: {data}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            if (data.Name != gameObject.name)
            {
                GameLogger.AddMessage($"Attempted to set data from another game object. Instance name: {gameObject.name}, data name: {data.Name}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            return true;
        }
    }
}
