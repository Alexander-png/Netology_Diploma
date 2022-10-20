using Platformer3d.LevelEnvironment.Mechanisms.Animations;
using Platformer3d.LevelEnvironment.Switchers;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Mechanisms.Doors
{
	public class TwoDoorGate : Gate, ISwitcherTarget
	{
		[SerializeField]
		private TwoDoorAnimation _animation;

		private bool _opened;

        public override bool Opened
		{
			get => _opened; 
			set
			{
				_opened = value;
				if (_animation != null) _animation.SetOpened(value);
			}
		}

        public bool IsSwitchedOn 
		{
			get => Opened; 
			set => Opened = value;
		}

		private void Start()
        {
			if (_animation == null)
			{
				EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: animation not specified.", EditorExtentions.GameLogger.LogType.Warning);
				return;
			}
			_animation.InitState(_openedByDefault);
		}
    }
}