using Platformer3d.CharacterSystem.DataContainers;
using Platformer3d.CharacterSystem.Interactors;
using Platformer3d.CharacterSystem.Movement.Base;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Base
{
	public abstract class MoveableCharacter : Character
	{
		[SerializeField]
		private CharacterMovement _movementController;
        [SerializeField]
        private SwitchTriggerInteractor _interactor;

        private bool _handlingEnabled;

        public CharacterMovement MovementController => _movementController;

        public bool HandlingEnabled
        {
            get => _handlingEnabled;
            set
            {
                _handlingEnabled = value;
                _movementController.MovementEnabled = _handlingEnabled;
            }
        }

        public override void OnRespawn(CharacterDataContainer data)
        {
            base.OnRespawn(data);
            _movementController.SetVelocity(Vector3.zero);
        }
    }
}