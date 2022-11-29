using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer3d.Weapons
{
	public class PlayerWeapon : BasicWeapon
	{
        [SerializeField]
        private Transform _visual;
        [SerializeField]
        private Collider _hitCollder;
        [SerializeField]
        private Animator _animator;

        private void OnEnable() =>
            _owner.Respawning += OnPlayerRespawning;

        private void OnDisable() =>
            _owner.Respawning -= OnPlayerRespawning;

        private void Start()
        {
            ResetValues();
        }

        public void OnAttackPerformed(InputValue input) =>
            MakeHit();

        private void MakeHit()
        {
            _animator.SetFloat("Hit", 1);
            _visual.gameObject.SetActive(true);
            _hitCollder.enabled = true;
        }

        private void ResetValues()
        {
            _animator.SetFloat("Hit", 0);
            _visual.gameObject.SetActive(false);
            _hitCollder.enabled = false;
        }

        public void OnHitEnd() =>
            ResetValues();

        private void OnPlayerRespawning(object sender, System.EventArgs e)
        {
            ResetAnimator();
            ResetValues();
        }

        private void ResetAnimator()
        {
            _animator.enabled = false;
            _animator.enabled = true;
        }
    }
}