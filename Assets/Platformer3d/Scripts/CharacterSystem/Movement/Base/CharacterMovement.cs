using Platformer3d.LevelEnvironment.Elements.Common;
using Platformer3d.Scriptable.Characters;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Movement.Base
{
    public abstract class CharacterMovement : MonoBehaviour
	{
		[SerializeField]
		private MovementStats _defaultMovementStats;
		[SerializeField]
		private Rigidbody _body;

        private MovementStatsInfo _movementStats;
        private Vector3 _currentCollisionNormal;

        private float _moveInput;
        private float _jumpInput;
        private float _dashInput;
        public bool IsJumpPerformed { get; set; }
        public bool IsDashPerformed { get; set; }

        protected int JumpsLeft { get; set; }

        public bool CanJump => JumpsLeft > 0;
        public float Acceleration => _movementStats.Acceleration;
        public float MaxSpeed => _movementStats.MaxSpeed;
        public float JumpForce => _movementStats.GetJumpForce(JumpsLeft);
        public float MaxJumpForce => _movementStats.MaxJumpForce;
        public float ClimbForce => _movementStats.ClimbForce;
        public float WallClimbRepulsion => _movementStats.WallClimbRepulsion;
        public float DashForce => _movementStats.DashForce;
        public float DashDuration => _movementStats.DashDuration;
        public float DashRechargeTime => _movementStats.DashRechargeTime;

        public bool OnGround { get; protected set; }
        public bool OnWall { get; protected set; }
        public bool InAir => _currentCollisionNormal == Vector3.zero;
        public bool MovementEnabled { get; set; }

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
                IsJumpPerformed = JumpInput >= 0.01f;
            }
        }

        public float DashInput
        {
            get => _dashInput;
            set
            {
                _dashInput = value;
                IsDashPerformed = _dashInput >= 0.01f && CheckCanDash();
            }
        }

        public virtual Vector3 Velocity
        {
            get => _body.velocity;
            set => _body.velocity = value;
        }

        protected virtual void Awake()
        {
            _movementStats = _defaultMovementStats.GetData();
            _currentCollisionNormal = Vector3.zero;
            ResetJumpCounter();
        }

        protected virtual void OnDisable()
        {
            ResetState();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Platform plat))
            {
                var newNormal = collision.GetContact(0).normal;
                
                if (_currentCollisionNormal != Vector3.zero)
                {
                    if (newNormal.x != 0 && _currentCollisionNormal.y != 0)
                    {
                        OnWall = true;
                        OnGround = false;
                        _currentCollisionNormal = newNormal;
                    }
                }
                else
                {
                    _currentCollisionNormal = newNormal;
                    OnWall = plat.Climbable ? Mathf.Abs(_currentCollisionNormal.x) > 0.9 : false;
                    OnGround = !OnWall;
                }

                if ((OnGround || OnWall) && plat.Climbable)
                {
                    ResetJumpCounter();
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Platform _))
            {
                _currentCollisionNormal = Vector3.zero;
            }
        }

        private bool CheckCanDash()
        {
            if (MoveInput == 0)
            {
                return false;
            }
            return DashForce != 0 && DashDuration != 0;
        }

        public virtual void AddStats(MovementStatsInfo stats)
        {
            _movementStats += stats;
            ResetJumpCounter();
        }

        public virtual void RemoveStats(MovementStatsInfo stats)
        {
            _movementStats -= stats;
            ResetJumpCounter();
        }

        private void ResetJumpCounter()
        {
            JumpsLeft = _movementStats.JumpCountInRow;
        }

        protected virtual void ResetState()
        {
            ResetJumpCounter();
        }
    }
}