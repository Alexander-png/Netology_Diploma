using UnityEngine;

namespace Platformer3d.LevelEnvironment.Mechanisms.Switchers
{
	public abstract class Switcher : MonoBehaviour
	{
		[SerializeField]
		protected bool _isSwitchedOn;

		public abstract bool IsSwitchedOn { get; set; } 
	}
}