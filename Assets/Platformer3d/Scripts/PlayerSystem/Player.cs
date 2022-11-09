using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.DataContainers;
using Platformer3d.CharacterSystem.Enums;
using Platformer3d.GameCore;
using Platformer3d.Scriptable.Characters;
using Platformer3d.SkillSystem;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Platformer3d.PlayerSystem
{
    public class Player : MoveableCharacter, IDamagableCharacter, ISkillObservable, ISaveable
    {
        [Inject]
        private GameSystem _gameSystem;

        [SerializeField]
        private Inventory _inventory;
        [SerializeField]
        private SkillObserver _skillObserver;

        private bool _damageImmune = false;
        private float _damageImmuneTime;
        private float _currentHealh;
        private float _maxHealth;

        public float CurrentHealth => _currentHealh;

        public Inventory Inventory => _inventory;
        public SkillObserver SkillObserver => _skillObserver;

        public event EventHandler Died;

        private class PlayerData : SaveData
        {
            public SideTypes Side;
            public Vector3 Position;
            public float CurrentHealth;
        }

        protected override void Start()
        {
            _gameSystem.RegisterSaveableObject(this);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _damageImmune = false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopAllCoroutines();
        }

        protected override void SetDefaultParameters(DefaultCharacterStats stats)
        {
            base.SetDefaultParameters(stats);
            _maxHealth = stats.MaxHealth;
            _currentHealh = _maxHealth;
            _damageImmuneTime = stats.DamageImmuneTime;
        }

        public override void SetDataFromContainer(CharacterDataContainer data)
        {
            base.SetDataFromContainer(data);
            PlayerDataContainer playerData = data as PlayerDataContainer;
            _currentHealh = playerData.CurrentHealth;
        }

        public override CharacterDataContainer GetDataAsContainer() =>
            new PlayerDataContainer()
            {
                Side = Side,
                Name = Name,
                Position = transform.position,
                CurrentHealth = _currentHealh,
            };

        public void SetDamage(float damage, Vector3 pushVector)
        {
            if (_damageImmune)
            {
                return;
            }

            StartCoroutine(DamageImmuneCoroutine(_damageImmuneTime));
            MovementController.SetVelocity(pushVector);
            _currentHealh = Mathf.Clamp(_currentHealh - damage, 0, _maxHealth);
            if (_currentHealh < 0.01f)
            {
                Died?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Heal(float value) =>
            _currentHealh = Mathf.Clamp(_currentHealh + value, 0, _maxHealth);

        private IEnumerator DamageImmuneCoroutine(float time)
        {
            _damageImmune = true;
            yield return new WaitForSeconds(time);
            _damageImmune = false;
        }

        private bool ValidateData(PlayerData data)
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

        public object GetData() => new PlayerData()
        {
            Name = gameObject.name,
            Side = Side,
            Position = transform.position,
            CurrentHealth = CurrentHealth
        };

        public void SetData(object data)
        {
            PlayerData dataToSet = data as PlayerData;
            if (!ValidateData(dataToSet))
            {
                return;
            }

            Side = dataToSet.Side;
            transform.position = dataToSet.Position;
            _currentHealh = dataToSet.CurrentHealth;
        }
    }
}