using Newtonsoft.Json.Linq;
using Platformer3d.GameCore;
using Platformer3d.Interaction;
using UnityEngine;
using Zenject;

namespace Platformer3d.LevelEnvironment.Mechanisms.Switchers
{
    public abstract class Switcher : MonoBehaviour, ISwitcher, ISaveable
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

		protected virtual void Start() =>
			GameSystem.RegisterSaveableObject(this);

		private bool ValidateData(JObject data)
		{
			if (data == null)
			{
				EditorExtentions.GameLogger.AddMessage($"Failed to cast data. Instance name: {gameObject.name}, data type: {data}", EditorExtentions.GameLogger.LogType.Error);
				return false;
			}
			if (data.Value<string>("Name") != gameObject.name)
			{
				EditorExtentions.GameLogger.AddMessage($"Attempted to set data from another game object. Instance name: {gameObject.name}, data name: {data.Value<string>("Name")}", EditorExtentions.GameLogger.LogType.Error);
				return false;
			}
			return true;
		}

        public JObject GetData()
        {
            var data = new JObject();
            data["Name"] = gameObject.name;
            data["IsSwitchedOn"] = _isSwitchedOn;
            data["ActionId"] = _actionId;
            data["WasSwitched"] = WasSwitched;
            data["IsOneOff"] = IsOneOff;
            return data;
        }

        public bool SetData(JObject data)
        {
            if (!ValidateData(data))
            {
                return false;
            }
			Reset(data);
			return true;
        }

		protected virtual void Reset(JObject data)
		{
			_isOneOff = false;
            _isSwitchedOn = data.Value<bool>("IsSwitchedOn");
            _actionId = data.Value<string>("ActionId");
            WasSwitched = data.Value<bool>("WasSwitched");
            _isOneOff = data.Value<bool>("IsOneOff");
		}
	}
}