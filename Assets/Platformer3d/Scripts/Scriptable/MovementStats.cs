using UnityEngine;

namespace Platformer3d.Scriptable
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/MovementStats")]
	public class MovementStats : ScriptableObject
	{
		[SerializeField]
		private float _maxMoveSpeed;
		[SerializeField]
		private float _jumpForce;
	}
}