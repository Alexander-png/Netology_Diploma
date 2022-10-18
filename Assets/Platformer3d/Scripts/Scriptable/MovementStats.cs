using Platformer3d.CharacterSystem.Movement.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platformer3d.Scriptable
{
	/// <summary>
	/// Use this to create default movement stats
	/// </summary>
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Movement stats")]
	public class MovementStats : ScriptableObject
	{
		[SerializeField]
		private float _maxSpeed;
		[SerializeField]
		private float _acceleration;
		[SerializeField]
		private float[] _jumps;
		[SerializeField]
		private int _jumpCountInRow;
		[SerializeField]
		private float _climbForce;
		[SerializeField]
		private float _wallClimbRepulsion;
		[SerializeField]
		private float _dashDistance;

        public MovementStatsInfo GetData() => new MovementStatsInfo()
		{
			MaxSpeed = _maxSpeed,
			Acceleration = _acceleration,
			Jumps = new List<float>(_jumps), // need for jumps initialization
			JumpCountInRow = _jumpCountInRow,
			ClimbForce = _climbForce,
			WallClimbRepulsion = _wallClimbRepulsion,
			DashDistance = _dashDistance,
		};
	}
}