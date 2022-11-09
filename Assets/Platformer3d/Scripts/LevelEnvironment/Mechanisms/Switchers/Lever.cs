using Platformer3d.LevelEnvironment.Mechanisms.Animations;
using Platformer3d.LevelEnvironment.Switchers;
using System;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Mechanisms.Switchers
{
	public class Lever : Switcher
	{
		[SerializeField]
		private GameObject _target;
		[SerializeField]
		private LeverSwitchAnimation _switchAnimator;

		private ISwitcherTarget _switcher;

		public override bool IsSwitchedOn
		{
			get => _isSwitchedOn;
			set
			{
				if (!CanPerform)
                {
					return;
                }

				if (_isSwitchedOn != value)
                {
					WasSwitched = true;
				}

				_isSwitchedOn = value;

				if (_switchAnimator != null) _switchAnimator.Switch(value);
				if (_switcher != null) 
				{
					if (!ShowTargetBeforeSwitch)
                    {
						_switcher.IsSwitchedOn = value; 
                    }
					else
                    {
						GameSystem.ShowAreaUntilActionEnd(_switcher.FocusPoint, new Action(() => _switcher.IsSwitchedOn = value), _switcher.SwitchTime);
                    }
				} 
			}
		}

        protected override void Start()
        {
			base.Start();
			if (_target == null)
            {
                EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: target not specified.", EditorExtentions.GameLogger.LogType.Warning);
                return;
            }

			_switcher = _target.GetComponent<ISwitcherTarget>();
            if (_switcher == null)
            {
                EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: the {_target.gameObject.name} does not contain ISwitcherTarget component.", EditorExtentions.GameLogger.LogType.Error);
				return;
            }

			_switcher.IsSwitchedOn = IsSwitchedOn;

			if (_switchAnimator == null)
            {
				EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: animation not specified.", EditorExtentions.GameLogger.LogType.Warning);
				return;
			}
			_switchAnimator.InitState(IsSwitchedOn);
        }

        protected override void Reset(SwitcherData data)
        {
            base.Reset(data);
			if (_switchAnimator != null)
            {
				_switchAnimator.InitState(IsSwitchedOn);
			}
		}
    }
}