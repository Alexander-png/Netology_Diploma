using UnityEngine;

namespace Platformer3d.LevelEnvironment.Elements.Switchers
{
	public abstract class LevelElementSwitcher : MonoBehaviour
	{
		public abstract void Switch(bool value);
	}
}