using Platformer3d.CharacterSystem.Enums;
using UnityEngine;

namespace Platformer3d.CharacterSystem.DataContainers
{
	public class CharacterDataContainer
	{
		public string Name { get; set; }
		public SideTypes Side { get; set; }
		public Vector3 Position { get; set; }
	}

	public class PlayerDataContainer : CharacterDataContainer
    {
		public float CurrentHealth { get; set; }
    }
}