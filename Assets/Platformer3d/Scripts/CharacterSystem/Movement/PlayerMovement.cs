using Platformer3d.CharacterSystem.Movement.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer3d.CharacterSystem.Movement
{
	public class PlayerMovement : CharacterMovement
	{
        private float _moveAxis;
        private bool _isJumpPerformed;

        private void OnDisable()
        {
            _moveAxis = 0;
        }

        private void FixedUpdate()
        {
            Move();
            Jump();
        }

        private void OnRun(InputValue input)
        {
            _moveAxis = input.Get<float>();
        }

		private void OnJump(InputValue input)
        {
            _isJumpPerformed = input.Get<float>() >= 0.01f;
        }

        private void Move()
        {
            Vector2 velocity = Body.velocity;

            //velocity.x += Acceleration * _moveAxis * TimeSystem.Instance.ScaledGameFixedDeltaTime;
            velocity.x += Acceleration * _moveAxis * Time.deltaTime;
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
                    if (velocity.y < 0)
                    {
                        velocity.y = 0;
                    }
                    velocity.y = Mathf.Clamp(velocity.y + JumpForce, 0, MaxJumpForce);
                    JumpsLeft -= 1;
                }
                else if (OnWall)
                {
                    velocity.x += WallClimbRepulsion * -Mathf.Sign(_moveAxis);
                    velocity.y += ClimbForce;
                    JumpsLeft -= 1;
                }

                _isJumpPerformed = false;
                Body.velocity = velocity;
            }
        }
    }
}