using UnityEngine;

namespace Platformer3d.LevelEnvironment.Elements.Common
{
	public class Platform : BaseLevelElement
	{
		[SerializeField]
		private bool _climbable;

		public bool Climbable => _climbable;
	}
}