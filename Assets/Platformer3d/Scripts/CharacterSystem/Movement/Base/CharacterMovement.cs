using Platformer3d.Interactables.Elements.Base;
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

        public bool OnGround { get; protected set; }
        public bool OnWall { get; protected set; }
        public bool InAir { get; private set; }

        protected int JumpsLeft { get; set; }

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
            ResetJumpCounter();
        }

        protected virtual void OnDisable()
        {
            ResetState();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out BaseLevelElement _))
            {
                var normal = collision.GetContact(0).normal;

                OnGround = normal.y == 1;
                OnWall = normal.x != 0;
                if (OnGround || OnWall)
                {
                    ResetJumpCounter();
                }
                InAir = false;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out BaseLevelElement _))
            {
                InAir = true;
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