using Platformer3d.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer3d.LevelEnvironment.Mechanisms.Switchers
{
	public abstract class Switcher : MonoBehaviour
	{
		[Inject]
		private GameSystem _gameSystem;
		[SerializeField]
		protected bool _isSwitchedOn;

		[SerializeField]
		private bool _showTargetBeforeSwitch;

		public GameSystem GameSystem => _gameSystem;
		public bool ShowTargetBeforeSwitch => _showTargetBeforeSwitch;

		public abstract bool IsSwitchedOn { get; set; } 
	}
}