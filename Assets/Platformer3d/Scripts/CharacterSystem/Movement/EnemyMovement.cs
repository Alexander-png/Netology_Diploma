using Platformer3d.CharacterSystem.Movement.Base;
using System.Collections;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Movement
{
	public class EnemyMovement : CharacterMovement
	{
        private float _moveInput;
        private float _jumpInput;
        private float _dashInput;

        private bool _dashCharged = true;
        private bool _isDashPerformed;
        private bool _inDash;
        private float _dashDirection;

        private bool _isJumpPerformed;

        public float MoveInput 
        {
            get => _moveInput; 
            set => _moveInput = value; 
        }

        public float JumpInput
        {
            get => _jumpInput;
            set
            {
                _jumpInput = value;
                _isJumpPerformed = JumpInput >= 0.01f;
            }
        }

        public float DashInput
        {
            get => _dashInput;
            set
            {
                _dashInput = value;
                _isDashPerformed = _dashInput >= 0.01f && CheckCanDash();
            }
        }

        private void FixedUpdate()
        {
            if (MovementEnabled)
            {
                Move();
                Jump();
            }
        }

        protected override void ResetState()
        {
            _moveInput = 0;
            _inDash = false;
            _dashCharged = true;
            StopAllCoroutines();
        }

        private bool CheckCanDash()
        {
            if (_moveInput == 0)
            {
                return false;
            }
            return DashForce != 0 && DashDuration != 0;
        }

        private void Move()
        {
            Vector2 velocity = Body.velocity;
            if (!_isDashPerformed && !_inDash)
            {
                velocity.x += Acceleration * MoveInput * Time.deltaTime;
                velocity.x = Mathf.Clamp(velocity.x, -MaxSpeed, MaxSpeed);
            }
            else
            {
                if (_isDashPerformed && !_inDash && _dashCharged)
                {
                    StartCoroutine(DashMove(DashDuration));
                    _dashDirection = Mathf.Sign(MoveInput);
                    _isDashPerformed = false;
                }
                if (_inDash)
                {
                    velocity.x = DashForce * Mathf.Sign(_dashDirection);
                }
            }
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
                    velocity.x += WallClimbRepulsion * -Mathf.Sign(MoveInput);
                    velocity.y += ClimbForce;
                    JumpsLeft -= 1;
                }

                _isJumpPerformed = false;
                Body.velocity = velocity;
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