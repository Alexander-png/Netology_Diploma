using Platformer3d.CharacterSystem.Base;
using Platformer3d.Scriptable.LevelElements;
using UnityEngine;

namespace Platformer3d.Weapons
{
	public class BasicWeapon : MonoBehaviour
	{
        [SerializeField]
        private DamageStats _damageStats;

		private Character _owner;
		protected virtual void Start()
        {
			_owner = transform.parent.GetComponent<Character>();
        }

		protected virtual void OnTriggerStay(Collider other)
        {
			if (other.TryGetComponent(out IDamagableCharacter target))
            {
				if (!target.Equals(_owner))
                {
                    Vector3 newVelocity = (-other.attachedRigidbody.velocity + other.transform.up).normalized;
                    newVelocity *= _damageStats.PushForce;
                    target.SetDamage(_damageStats.Damage, newVelocity);
                }
            }
        }
    }
}