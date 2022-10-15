using Platformer3d.CharacterSystem.Movement.Base;
using UnityEngine.InputSystem;

namespace Platformer3d.CharacterSystem.Movement
{
	public class PlayerMovement : CharacterMovement
	{
        private float _moveAxis;
        private bool _isJumpPerformed;

		private void OnRun(InputValue input)
        {
            _moveAxis = input.Get<float>();
        }

		private void OnJump(InputValue input)
        {
            _isJumpPerformed = input.Get<float>() >= 0.01f;
        }

        private void FixedUpdate()
        {
            MoveBody();
        }

        private void MoveBody()
        {

        }
    }
}