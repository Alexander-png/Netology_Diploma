using UnityEngine;

namespace Platformer3d.Scriptable.Skills.Configurations
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Movement skill")]
	public class MovementSkillConfiguration : SkillConfiguration
	{
		[SerializeField]
		private float _maxSpeed;
		[SerializeField]
		private float _acceleration;
		[SerializeField]
		private float _jumpCountInRow;
		[SerializeField]
		private float _climbForce;
		[SerializeField]
		private float _wallClimbRepulsion;
		[SerializeField]
		private float _dashDistance;

		public float MaxSpeed => _maxSpeed;
		public float Acceleration => _acceleration;
		public float JumpCountInRow => _jumpCountInRow;
		public float ClimbForce => _climbForce;
		public float DashDistance => _dashDistance;
	}
}