using Platformer3d.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer3d.ActivitySystem
{
	public abstract class ActivityObserver : MonoBehaviour
	{
		[Inject]
		private GameSystem _gameSystem;

        [SerializeField]
		private bool _isOneOff = false;

		protected bool _inAction;
		protected bool _activityEnded;

		protected bool IsOneOff => _isOneOff;
		protected GameSystem GameSystem => _gameSystem;

		protected virtual void StartActivity() { }
		protected virtual void EndActivity() { }
		protected virtual void ResetActivity() { }
	}
}