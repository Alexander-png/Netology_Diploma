using Platformer3d.Interactables.Animators;
using Platformer3d.LevelEnvironment.Elements.Switchers;
using UnityEngine;

namespace Platformer3d.Interactables.Switchers
{
	public class Lever : Switcher
	{
		[SerializeField]
		private LevelElementSwitcher _target;
        [SerializeField]
		private LeverSwitchAnimation _switchAnimator;

        private void Start()
        {
			_target.Switch(IsSwitchedOn);
			_switchAnimator.InitState(IsSwitchedOn);
        }

        public override bool IsSwitchedOn 
		{ 
			get => _isSwitchedOn;
			set 
			{ 
				_isSwitchedOn = value;
				_target.Switch(value);
				_switchAnimator.Switch(value);
			} 
		}
	}
}