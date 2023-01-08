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

        private class CollectableItemData : SaveData
        {
            public string ItemID;
            public bool Collected;
        }

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

        private bool ValidateData(CollectableItemData data)
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

        public object GetData() => new CollectableItemData()
        {
            Name = gameObject.name,
            ItemID = _itemId,
            Collected = !gameObject.activeSelf,
        };

        public bool SetData(object data)
        {
            CollectableItemData dataToSet = data as CollectableItemData;
            if (!ValidateData(dataToSet))
            {
                return false;
            }

            _itemId = dataToSet.ItemID;
            gameObject.SetActive(!dataToSet.Collected);
            return true;
        }

        public bool SetData(JObject data) => 
            SetData(data.ToObject<CollectableItemData>());
    }
}