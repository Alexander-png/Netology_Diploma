using Platformer3d.CharacterSystem.Enums;
using UnityEngine;

namespace Platformer3d.Scriptable.Characters
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Character stats")]
	public class DefaultCharacterStats : ScriptableObject
	{
		// TODO: split this class by character types
		[SerializeField]
		private string _name;
		[SerializeField]
		private SideTypes _side;
		[SerializeField]
		private float _maxHealth;
		[SerializeField]
		private float _maxMana;
		[SerializeField]
		private float _damageImmuneTime;
		[SerializeField]
		private bool _immortal;
		[SerializeField]
		private bool _interactable;
		
		public string Name => _name;
		public SideTypes Side => _side;
		public float MaxHealth => _maxHealth;
		public float MaxMana => _maxMana;
		public float DamageImmuneTime => _damageImmuneTime;
		public bool Immortal => _immortal;
		public bool Interactable => _interactable;
    }
}