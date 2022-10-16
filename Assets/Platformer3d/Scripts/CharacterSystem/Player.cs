using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.DataContainers;
using Platformer3d.GameCore;
using Platformer3d.Scriptable;
using System;
using System.Collections;
using UnityEngine;

namespace Platformer3d.CharacterSystem
{
    public class Player : MoveableCharacter, IDamagableCharacter
    {
        private bool _damageImmune = false;
        private float _damageImmuneTime;
        private float _currentHealh;
        private float _maxHealth;

        public float CurrentHealth => _currentHealh;

        public event EventHandler OnDied;

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

        public override void SetData(CharacterDataContainer data)
        {
            base.SetData(data);
            PlayerDataContainer playerData = data as PlayerDataContainer;
            _currentHealh = playerData.CurrentHealth;
        }

        public override CharacterDataContainer GetData() =>
            new PlayerDataContainer()
            {
                Side = Side,
                Name = Name,
                Position = transform.position,
                CurrentHealth = _currentHealh,
            };

        public void OnGotDamage(float damage, float pushForce)
        {
            if (_damageImmune)
            {
                return;
            }

            StartCoroutine(DamageImmuneCoroutine(_damageImmuneTime));
            MovementController.SetVelocity(CalculateHitPushBackSpeed(pushForce));
            _currentHealh = Mathf.Clamp(_currentHealh - damage, 0, _maxHealth);
            if (_currentHealh < 0.01f)
            {
                OnDied?.Invoke(this, EventArgs.Empty);
            }
        }

        private Vector3 CalculateHitPushBackSpeed(float pushForce) =>
            (-MovementController.Body.velocity + Vector3.up).normalized * pushForce;

        public void Heal(float value) =>
            _currentHealh = Mathf.Clamp(_currentHealh + value, 0, _maxHealth);

        private IEnumerator DamageImmuneCoroutine(float time)
        {
            _damageImmune = true;
            while (time > 0)
            {
                yield return null;
                time -= TimeSystem.Instance.ScaledGameDeltaTime;
            }
            _damageImmune = false;
        }
    }
}