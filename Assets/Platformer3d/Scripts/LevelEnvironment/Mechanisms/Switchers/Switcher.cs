using Platformer3d.GameCore;
using Platformer3d.Interaction;
using UnityEngine;
using Zenject;

namespace Platformer3d.LevelEnvironment.Mechanisms.Switchers
{
	public abstract class Switcher : MonoBehaviour, ISwitcher
	{
		[Inject]
		private GameSystem _gameSystem;

		[SerializeField]
		private string _actionId;
		[SerializeField]
		protected bool _isSwitchedOn;
		[SerializeField]
		private bool _isOneOff;
		[SerializeField]
		private bool _showTargetBeforeSwitch;

		public GameSystem GameSystem => _gameSystem;
		public bool ShowTargetBeforeSwitch => _showTargetBeforeSwitch;

		public abstract bool IsSwitchedOn { get; set; }

		public string ActionId => _actionId;
		public bool WasSwitched { get; protected set; }
		public bool IsOneOff => _isOneOff;
		public virtual bool CanPerform => !(WasSwitched && _isOneOff);
	}
}