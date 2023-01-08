using Newtonsoft.Json.Linq;
using Platformer3d.LevelEnvironment.Mechanisms.Animations;
using Platformer3d.LevelEnvironment.Switchers;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Mechanisms.Doors
{
	public class TwoDoorGate : Gate, ISwitcherTarget
	{
		[SerializeField]
		private TwoDoorAnimation _animation;

		private bool _isOpened;

        public override bool IsOpened
		{
			get => _isOpened; 
			set
			{
				_isOpened = value;
				if (_animation != null) _animation.SetOpened(value);
			}
		}

        public bool IsSwitchedOn 
		{
			get => IsOpened; 
			set => IsOpened = value;
		}

        public float SwitchTime => _animation != null ? _animation.AnimationTime : 0f;

        public Transform FocusPoint => CameraFocusPoint;

        private void Start()
        {
			GameSystem.RegisterSaveableObject(this);

			if (_animation == null)
			{
				EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: animation not specified.", EditorExtentions.GameLogger.LogType.Warning);
				return;
			}
			_animation.InitState(_openedByDefault);
		}

		public override object GetData() => new GateData()
		{
			Name = gameObject.name,
			IsOpened = IsOpened
		};

        public override bool SetData(object data)
		{
			GateData dataToSet = data as GateData;

			if (!ValidateData(dataToSet))
			{
				return false;
			}
			_animation.InitState(dataToSet.IsOpened);
			return true;
		}

		public override bool SetData(JObject data) => 
			SetData(data.ToObject<GateData>());
    }
}