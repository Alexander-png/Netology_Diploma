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

		protected class SwitcherData : SaveData
		{
			public bool IsSwitchedOn;
			public string ActionId;
			public bool WasSwitched;
			public bool IsOneOff;
		}

		protected virtual void Start()
        {
			GameSystem.RegisterSaveableObject(this);
		}

		public virtual object GetData() => new SwitcherData()
	    {
	    	Name = gameObject.name,
	    	IsSwitchedOn = _isSwitchedOn,
	    	ActionId = _actionId,
	    	WasSwitched = WasSwitched,
	    	IsOneOff = IsOneOff,
	    };

		protected virtual bool ValidateData(SwitcherData data)
        {
			if (data == null)
			{
				EditorExtentions.GameLogger.AddMessage($"Failed to cast data. Instance name: {gameObject.name}, data type: {data}", EditorExtentions.GameLogger.LogType.Error);
				return false;
			}
			if (data.Name != gameObject.name)
			{
				EditorExtentions.GameLogger.AddMessage($"Attempted to set data from another game object. Instance name: {gameObject.name}, data name: {data.Name}", EditorExtentions.GameLogger.LogType.Error);
				return false;
			}
			return true;
		}

        public virtual bool SetData(object data)
        {
			SwitcherData dataToSet = data as SwitcherData;
			if (!ValidateData(dataToSet))
            {
				return false;
            }
			Reset(dataToSet);
			return true;
		}

		public bool SetData(JObject data) => 
			SetData(data.ToObject<SwitcherData>());

		protected virtual void Reset(SwitcherData data)
		{
			_isOneOff = false;

			_isSwitchedOn = data.IsSwitchedOn;
			_actionId = data.ActionId;
			WasSwitched = data.WasSwitched;
			_isOneOff = data.IsOneOff;
		}
	}
}