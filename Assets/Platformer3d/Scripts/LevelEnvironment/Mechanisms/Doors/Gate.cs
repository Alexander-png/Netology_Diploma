using Newtonsoft.Json.Linq;
using Platformer3d.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer3d.LevelEnvironment.Mechanisms.Doors
{
    public abstract class Gate : MonoBehaviour, ISaveable
	{
        [Inject]
		private GameSystem _gameSystem;

		[SerializeField, Tooltip("Notice: if this object is handled by another object, this field will not work.")]
		protected bool _openedByDefault;

		[SerializeField]
		private Transform _cameraFocusPoint;

		protected Transform CameraFocusPoint => _cameraFocusPoint;
		protected GameSystem GameSystem => _gameSystem;

		public abstract bool IsOpened { get; set; }

		protected class GateData : SaveData
		{
			public bool IsOpened;
		}

		protected virtual void OnDrawGizmos()
        {
			if (CameraFocusPoint == null)
			{
				return;
			}

			Color color = Color.cyan;
			color.a = 0.5f;
			Gizmos.color = color;
			Gizmos.DrawSphere(CameraFocusPoint.position, 1f);
		}

		protected virtual bool ValidateData(GateData data)
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

		public abstract object GetData();
		public abstract bool SetData(object data);
		public abstract bool SetData(JObject data);
    }
}