using Platformer3d.CharacterSystem.DataContainers;
using Platformer3d.CharacterSystem.Movement.Base;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Base
{
	public abstract class MoveableCharacter : Character
	{
		[SerializeField]
		private CharacterMoveController _movementController;

		public CharacterMoveController MovementController => _movementController;

        public override void OnRespawn(CharacterDataContainer data)
        {
            base.OnRespawn(data);
            _movementController.SetVelocity(Vector3.zero);
        }
    }
}