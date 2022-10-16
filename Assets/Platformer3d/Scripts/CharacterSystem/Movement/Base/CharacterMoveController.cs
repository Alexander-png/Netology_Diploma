using Platformer3d.LevelEnvironment.Platforms;
using Platformer3d.Scriptable;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Movement.Base
{
	public abstract class CharacterMoveController : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody _body;
		[SerializeField]
		private MovementStats _baseMovementStats;

        public bool OnGround { get; protected set; }
        public bool OnWall { get; protected set; }
        public bool InAir { get; private set; }

        protected int JumpsLeft { get; set; }
        protected bool CanJump => JumpsLeft > 0;

        protected float Acceleration => _baseMovementStats.Acceleration;
        protected float MaxSpeed => _baseMovementStats.MaxSpeed;
        protected float JumpForce => _baseMovementStats.GetJumpForce(JumpsLeft);
        protected float ClimbForce => _baseMovementStats.ClimbForce;
        protected float WallClimbRepulsion => _baseMovementStats.WallClimbRepulsion;

		public Rigidbody Body => _body;

		public virtual void ApplyModifiers()
        {
            EditorExtentions.GameLogger.AddMessage("TODO: ApplyModifiers", EditorExtentions.GameLogger.LogType.Fatal);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<BaseLevelSegment>(out _))
            {
                var normal = collision.GetContact(0).normal;

                OnGround = normal.y == 1;
                OnWall = normal.x != 0;
                if (OnGround || OnWall)
                {
                    JumpsLeft = _baseMovementStats.JumpCountInRow;
                }
                InAir = false;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<BaseLevelSegment>(out _))
            {
                InAir = true;
            }
        }
    }
}