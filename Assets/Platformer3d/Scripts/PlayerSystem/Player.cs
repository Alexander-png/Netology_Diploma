using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.DataContainers;
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
        private float _currentHealth;
        private float _maxHealth;

        public float CurrentHealth => _currentHealth;

        public Inventory Inventory => _inventory;
        public SkillObserver SkillObserver => _skillObserver;

        public event EventHandler Died;

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
            _currentHealth = _maxHealth;
            _damageImmuneTime = stats.DamageImmuneTime;
        }

        public override void SetDataFromContainer(CharacterDataContainer data)
        {
            base.SetDataFromContainer(data);
            PlayerDataContainer playerData = data as PlayerDataContainer;
            _currentHealth = playerData.CurrentHealth;
        }

        public override CharacterDataContainer GetDataAsContainer() =>
            new PlayerDataContainer()
            {
                Side = Side,
                Name = Name,
                Position = transform.position,
                CurrentHealth = _currentHealth,
            };

        public void SetDamage(float damage, Vector3 pushVector)
        {
            if (_damageImmune)
            {
                return;
            }

            StartCoroutine(DamageImmuneCoroutine(_damageImmuneTime));
            MovementController.Velocity = pushVector;
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
            if (_currentHealth < 0.01f)
            {
                Died?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Heal(float value) =>
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _maxHealth);

        private IEnumerator DamageImmuneCoroutine(float time)
        {
            _damageImmune = true;
            yield return new WaitForSeconds(time);
            _damageImmune = false;
        }

        public object GetData() => new CharacterData()
        {
            Name = gameObject.name,
            Side = Side,
            Position = transform.position,
            CurrentHealth = CurrentHealth
        };

        public void SetData(object data)
        {
            CharacterData dataToSet = data as CharacterData;
            if (!ValidateData(dataToSet))
            {
                return;
            }

            Side = dataToSet.Side;
            transform.position = dataToSet.Position;
            _currentHealth = dataToSet.CurrentHealth;
        }
    }
}