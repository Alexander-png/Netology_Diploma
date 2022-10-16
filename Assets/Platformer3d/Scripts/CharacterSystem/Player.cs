using Platformer3d.CharacterSystem.Base;
using Platformer3d.GameCore;
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

        public event EventHandler OnDied;

        protected override void OnDisable()
        {
            base.OnDisable();
            StopAllCoroutines();
        }

        protected override void FillStatsFields()
        {
            base.FillStatsFields();
            _currentHealh = Stats.MaxHealth;
            _damageImmuneTime = Stats.DamageImmuneTime;
        }

        public void OnGotDamage(float damage, float pushForce)
        {
            if (_damageImmune)
            {
                return;
            }

            StartCoroutine(DamageImmuneCoroutine(_damageImmuneTime));
            MovementController.SetVelocity(CalculateHitPushBackSpeed(pushForce));
            _currentHealh = Mathf.Clamp(_currentHealh - damage, 0, Stats.MaxHealth);
            if (_currentHealh < 0.01f)
            {
                DeathLogic();
            }
        }

        private void DeathLogic() => OnDied?.Invoke(this, EventArgs.Empty);

        private Vector3 CalculateHitPushBackSpeed(float pushForce) =>
            (-MovementController.Body.velocity + Vector3.up).normalized * pushForce;

        public void Heal(float value) =>
            _currentHealh = Mathf.Clamp(_currentHealh + value, 0, Stats.MaxHealth);

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