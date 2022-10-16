using UnityEngine;

namespace Platformer3d.Scriptable
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Danger level element stats")]
	public class DamageStats : ScriptableObject
	{
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _pushForce;
		[SerializeField]
		private bool _isFatalDamage;

		public float Damage => IsFatalDamage ? float.MaxValue : _damage;
		public float PushForce => _pushForce;
		public bool IsFatalDamage => _isFatalDamage;
	}
}