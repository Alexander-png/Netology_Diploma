using Platformer3d.CharacterSystem.Enums;
using UnityEngine;

namespace Platformer3d.Scriptable
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/CharacterStats")]
	public class CharacterStats : ScriptableObject
	{
		[SerializeField]
		private SideTypes _side;

		public SideTypes Side => _side;
    }
}