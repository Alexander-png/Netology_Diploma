using Platformer3d.CharacterSystem.Movement.Base;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer3d.CharacterSystem.Movement
{
	public class PlayerMovement : CharacterMovement
	{
        private bool _dashCharged = true;
        private bool _inDash;
        private float _dashDirection;

        private void FixedUpdate()
        {
            if (MovementEnabled)
            {
                Move();
                Jump();
            }
        }

        public void OnRunPerformed(InputValue input)
        {
            MoveInput = input.Get<float>();
        }

        public void OnDashPerformed(InputValue input)
        {
            IsDashPerformed = input.Get<float>() >= 0.01f && CheckCanDash();
        }

		public void OnJumpPerformed(InputValue input)
        {
            IsJumpPerformed = input.Get<float>() >= 0.01f;
        }

        protected override void ResetState()
        {
            MoveInput = 0;
            _inDash = false;
            _dashCharged = true;
            StopAllCoroutines();
        }

        private bool CheckCanDash()
        {
            if (MoveInput == 0)
            {
                return false;
            }
            return DashForce != 0 && DashDuration != 0;
        }

        private void Move()
        {
            Vector2 velocity = Velocity;
            if (!IsDashPerformed && !_inDash)
            {
                velocity.x += Acceleration * MoveInput * Time.deltaTime;
                velocity.x = Mathf.Clamp(velocity.x, -MaxSpeed, MaxSpeed);
            }
            else
            {
                if (IsDashPerformed && !_inDash && _dashCharged)
                {
                    StartCoroutine(DashMove(DashDuration));
                    _dashDirection = Mathf.Sign(MoveInput);
                    IsDashPerformed = false;
                }
                if (_inDash)
                {
                    velocity.x = DashForce * Mathf.Sign(_dashDirection);
                }
            }
            Velocity = velocity;
        }

        private void Jump()
        {
            if (CanJump && IsJumpPerformed)
            {
                Vector2 velocity = Velocity;
                
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
                    velocity.x += WallClimbRepulsion * -Mathf.Sign(MoveInput);
                    velocity.y += ClimbForce;
                    JumpsLeft -= 1;
                }

                IsJumpPerformed = false;
                Velocity = velocity;
            }
        }

        private IEnumerator DashMove(float time)
        {
            _inDash = true;
            yield return new WaitForSeconds(time);
            _inDash = false;
            StartCoroutine(RechargeDash(DashRechargeTime));
        }

        private IEnumerator RechargeDash(float time)
        {
            _dashCharged = false;
            yield return new WaitForSeconds(time);
            _dashCharged = true;
        }
    }
}