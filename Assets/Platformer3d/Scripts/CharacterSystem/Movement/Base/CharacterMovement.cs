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

		public Rigidbody Body => _body;

        protected virtual void Awake()
        {
            _movementStats = _defaultMovementStats.GetData();
            ResetJumpCounter();
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

        public void AddStats(MovementStatsInfo stats)
        {
            _movementStats += stats;
            ResetJumpCounter();
        }

        public void RemoveStats(MovementStatsInfo stats)
        {
            _movementStats -= stats;
            ResetJumpCounter();
        }

        private void ResetJumpCounter()
        {
            JumpsLeft = _movementStats.JumpCountInRow;
        }
    }
}