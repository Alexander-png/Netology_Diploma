using Platformer3d.CharacterSystem.Movement.Base;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Input
{
	public class PlayerInputHandler : MonoBehaviour
	{
		[SerializeField]
		private CharacterMovement _movementController;

		protected CharacterMovement MovementController => _movementController;

		[ContextMenu("Find Movement controller")]
		private void FindController()
		{
			_movementController = FindObjectOfType<CharacterMovement>();
		}
	}
}