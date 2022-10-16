using Platformer3d.CharacterSystem.Movement.Base;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Base
{
	public abstract class MoveableCharacter : Character
	{
		[SerializeField]
		private CharacterMoveController _movementController;

		public CharacterMoveController MovementController => _movementController;
	}
}