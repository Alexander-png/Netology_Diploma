using System.Linq;
using UnityEngine;

namespace Platformer3d.Scriptable
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Movement stats")]
	public class MovementStats : ScriptableObject
	{
		[SerializeField]
		private float _maxSpeed;
		[SerializeField]
		private float _acceleration;
		[SerializeField]
		private float[] _jumpForce;
		[SerializeField]
		private int _jumpCountInRow;
		[SerializeField]
		private float _climbForce;
		[SerializeField]
		private float _wallClimbRepulsion;

		public float MaxSpeed => _maxSpeed;
		public float Acceleration => _acceleration;
		public int JumpCountInRow => _jumpCountInRow;
		public float ClimbForce => _climbForce;
		public float WallClimbRepulsion => _wallClimbRepulsion;
		public float MaxJumpForce => _jumpForce.Max();

        public float GetJumpForce(int jumpsLeft) => _jumpForce[_jumpForce.Length - jumpsLeft - 1];
    }
}