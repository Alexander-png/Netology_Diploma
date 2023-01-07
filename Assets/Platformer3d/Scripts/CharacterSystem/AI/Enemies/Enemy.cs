using Newtonsoft.Json.Linq;
using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.Enums;
using Platformer3d.GameCore;
using Platformer3d.PlayerSystem;
using Platformer3d.Scriptable.Characters;
using System;
using UnityEngine;
using Zenject;

namespace Platformer3d.CharacterSystem.AI.Enemies
{
	public abstract class Enemy : MoveableCharacter, IDamagableCharacter, ISaveable
    {
		[Inject]
		protected GameSystem _gameSystem;

        // TODO: better to move these fields to scriptable object
        [SerializeField]
        protected float _idleTime;
        [SerializeField]
        protected float _attackRadius;
        [SerializeField]
        protected float _playerHeightDiffToJump;
        [SerializeField]
        protected float _closeToPlayerDistance;
        
        protected float _currentHealth;
        protected float _maxHealth;

        protected Player _player;

        protected bool _behaviourEnabled;
        protected bool _inIdle;
        protected bool _attackingPlayer = false;

        public event EventHandler Died;

        public float CurrentHealth => _currentHealth;

        protected override void Start()
        {
            _gameSystem.RegisterSaveableObject(this);

            _player = _gameSystem.GetPlayer();
            MovementController.MovementEnabled = true;
            SetBehaviourEnabled(true);
        }

        protected override void Update()
        {
            base.Update();
            UpdateBehaviour();
        }

        protected override void SetDefaultParameters(DefaultCharacterStats stats)
        {
            base.SetDefaultParameters(stats);
            _maxHealth = stats.MaxHealth;
            _currentHealth = _maxHealth;
        }

        protected abstract void UpdateBehaviour();

        protected void InvokeDiedEvent() => 
            Died?.Invoke(this, EventArgs.Empty);

        public bool SetBehaviourEnabled(bool value) =>
            _behaviourEnabled = value;

        public void SetDamage(float damage, Vector3 pushVector, bool forced = false)
        {
            MovementController.Velocity = pushVector;
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
            if (_currentHealth < 0.01f)
            {
                InvokeDiedEvent();
                SetBehaviourEnabled(false);
                EditorExtentions.GameLogger.AddMessage($"Enemy with name {gameObject.name} was. Killed. You can implement respawn system");
                gameObject.SetActive(false);
            }
        }

        public void Heal(float value) =>
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _maxHealth);

        public virtual JObject GetData()
        {
            var data = new JObject();
            data["Name"] = gameObject.name;
            data["Side"] = Convert.ToInt32(Side);
            JObject position = new JObject();
            position["x"] = transform.position.x;
            position["y"] = transform.position.y;
            position["z"] = transform.position.z;
            data["Position"] = position;
            data["CurrentHealth"] = CurrentHealth;
            data["AttackingPlayer"] = _attackingPlayer;
            data["InIdle"] = _inIdle;
            return data;
        }

        public virtual bool SetData(JObject data)
        {
            if (!ValidateData(data))
            {
                return false;
            }

            Side = (SideTypes)data.Value<byte>("Side");
            JObject position = data.Value<JObject>("Position");
            transform.position = new Vector3(
                position.Value<float>("x"),
                position.Value<float>("y"),
                position.Value<float>("z")
            );
            _currentHealth = data.Value<float>("CurrentHealth");
            _attackingPlayer = data.Value<bool>("AttackingPlayer");
            _inIdle = data.Value<bool>("InIdle");
            return true;
        }

        public void OnPlayerNearby()
        {
            _inIdle = false;
            _attackingPlayer = true;
        }

        public void OnPlayerRanAway() =>
            _attackingPlayer = false;
    }
}