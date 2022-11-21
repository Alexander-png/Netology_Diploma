using Platformer3d.CharacterSystem.Movement.Base;
using Platformer3d.LevelEnvironment.Elements.Common;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Elements.Usable
{
	public class Thrower : Platform
	{
		[SerializeField]
		private float _throwForce;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterMovement movement))
            {
                movement.Velocity = CalcThrowForce();
            }
        }

        private Vector3 CalcThrowForce() => transform.up * _throwForce;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, CalcThrowForce());
        }
    }
}