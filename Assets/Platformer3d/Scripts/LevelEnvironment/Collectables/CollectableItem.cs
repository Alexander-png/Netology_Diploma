using Newtonsoft.Json.Linq;
using Platformer3d.GameCore;
using Platformer3d.PlayerSystem;
using Platformer3d.QuestSystem;
using UnityEngine;
using Zenject;

namespace Platformer3d.LevelEnvironment.Collectables
{
	public class CollectableItem : MonoBehaviour, IInventoryItem, IQuestTarget, ISaveable
    {
		[Inject]
		private GameSystem _gameSystem;

		[SerializeField]
		private string _itemId;

        public string QuestTargetId => _itemId;
        public string ItemId => _itemId;

        private void Start() =>
            _gameSystem.RegisterSaveableObject(this);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _gameSystem.OnCollectalbeCollected(this);
                gameObject.SetActive(false);
            }
        }

        private bool ValidateData(JObject data)
        {
            if (data == null)
            {
                EditorExtentions.GameLogger.AddMessage($"Failed to cast data. Instance name: {gameObject.name}, data type: {data}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            if (data.Value<string>("Name") != gameObject.name)
            {
                EditorExtentions.GameLogger.AddMessage($"Attempted to set data from another game object. Instance name: {gameObject.name}, data name: {data.Name}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            return true;
        }

        public JObject GetData()
        {
            var data = new JObject();
            data["Name"] = gameObject.name;
            data["ItemId"] = _itemId;
            data["Collected"] = !gameObject.activeSelf;
            return data;
        }

        public bool SetData(JObject data)
        {
            if (!ValidateData(data))
            {
                return false;
            }
            _itemId = data.Value<string>("ItemId");
            gameObject.SetActive(!data.Value<bool>("Collected"));
            return true;
        }
    }
}