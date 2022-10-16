using Platformer3d.GameCore;
using Platformer3d.CharacterSystem.Movement.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer3d.CharacterSystem.Movement
{
	public class PlayerMovement : CharacterMoveController
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
            Move();
            Jump();
        }

        private void Move()
        {
            Vector2 velocity = Body.velocity;

            velocity.x += Acceleration * _moveAxis * TimeSystem.Instance.ScaledGameFixedDeltaTime;
            velocity.x = Mathf.Clamp(velocity.x, -MaxSpeed, MaxSpeed);
            Body.velocity = velocity;
        }

        private void Jump()
        {
            if (CanJump && _isJumpPerformed)
            {
                Vector2 velocity = Body.velocity;
                
                if (OnGround || InAir)
                {
                    JumpsLeft -= 1;
                    if (velocity.y < 0)
                    {
                        velocity.y = 0;
                    }
                    velocity.y = Mathf.Clamp(velocity.y + MaxJumpForce, 0, JumpForce);
                }
                else if (OnWall)
                {
                    JumpsLeft -= 1;
                    velocity.x += WallClimbRepulsion * -Mathf.Sign(_moveAxis);
                    velocity.y += ClimbForce;
                }

                _isJumpPerformed = false;
                Body.velocity = velocity;
            }
        }
    }
}