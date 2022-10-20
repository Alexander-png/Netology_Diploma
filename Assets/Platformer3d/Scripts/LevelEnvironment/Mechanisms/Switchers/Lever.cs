using Platformer3d.LevelEnvironment.Mechanisms.Animations;
using Platformer3d.LevelEnvironment.Triggers.Interactable;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Mechanisms.Switchers
{
	public class Lever : Switcher
	{
		[SerializeField]
		private GameObject _target;
        [SerializeField]
		private LeverSwitchAnimation _switchAnimator;

		private ISwitchTriggerTarget _switcher;

        private void Start()
        {
			if (_target == null)
            {
                EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: switcher target not specified.", EditorExtentions.GameLogger.LogType.Warning);
                return;
            }

			_switcher = _target.GetComponent<ISwitchTriggerTarget>();
            if (_switcher == null)
            {
                EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: the {_target.gameObject.name} does not contain switcher component.", EditorExtentions.GameLogger.LogType.Error);
				return;
            }

			_switcher.IsSwitchedOn = IsSwitchedOn;

			if (_switchAnimator == null)
            {
				EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: _switch animation not specified.", EditorExtentions.GameLogger.LogType.Warning);
				return;
			}
			_switchAnimator.InitState(IsSwitchedOn);
        }

        public override bool IsSwitchedOn 
		{ 
			get => _isSwitchedOn;
			set 
			{ 
				_isSwitchedOn = value;

				if (_switcher != null) _switcher.IsSwitchedOn = value;
				if (_switchAnimator != null) _switchAnimator.Switch(value);
			} 
		}
	}
}