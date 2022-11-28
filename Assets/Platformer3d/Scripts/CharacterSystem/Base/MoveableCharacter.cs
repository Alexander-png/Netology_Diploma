using Platformer3d.CharacterSystem.Movement.Base;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Base
{
	public abstract class MoveableCharacter : Character
	{
		[SerializeField]
		private CharacterMovement _movementController;

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

        protected override void Update()
        {
            UpdateVisual();
        }

        public override void NotifyRespawn()
        {
            base.NotifyRespawn();
            _movementController.Velocity = Vector3.zero;
        }

        protected virtual void UpdateVisual()
        {
            if (_movementController.MoveInput > 0f)
            {
                _visual.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (_movementController.MoveInput < 0f)
            {
                _visual.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
        }
    }
}