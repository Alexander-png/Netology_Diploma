using Platformer3d.LevelEnvironment.Elements.Common;
using Platformer3d.Scriptable;
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

        protected int JumpsLeft { get; set; }

        public bool OnGround { get; protected set; }
        public bool OnWall { get; protected set; }
        public bool InAir => _currentCollisionNormal == Vector3.zero;

        protected bool CanJump => JumpsLeft > 0;
        protected float Acceleration => _movementStats.Acceleration;
        protected float MaxSpeed => _movementStats.MaxSpeed;
        protected float JumpForce => _movementStats.GetJumpForce(JumpsLeft);
        protected float MaxJumpForce => _movementStats.MaxJumpForce;
        protected float ClimbForce => _movementStats.ClimbForce;
        protected float WallClimbRepulsion => _movementStats.WallClimbRepulsion;
        protected float DashForce => _movementStats.DashForce;
        protected float DashDuration => _movementStats.DashDuration;
        protected float DashRechargeTime => _movementStats.DashRechargeTime;

		public Rigidbody Body => _body;

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

        public virtual void SetVelocity(Vector3 velocity) =>
            _body.velocity = velocity;

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