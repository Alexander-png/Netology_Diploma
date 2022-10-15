using Platformer3d.Scriptable;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Movement.Base
{
	public abstract class CharacterMovement : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody _body;
		[SerializeField]
		private MovementStats _movementStats;
	}
}