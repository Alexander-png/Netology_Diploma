using Newtonsoft.Json.Linq;
using Platformer3d.GameCore;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Platformer3d.PlayerSystem
{
    public interface IInventoryItem 
    {
        public string ItemId { get; }
    }

    public class Inventory : MonoBehaviour, ISaveable
    {
        private const string SaveableEntityId = "player_Inventory";

        [Inject]
        private GameSystem _gameSystem;

        private List<string> _items;

        public IEnumerable<string> Items => new List<string>(_items);

        private class InventoryData : SaveData
        {
            public List<string> Items;
        }

        private void Awake() =>
            _items = new List<string>();

        private void Start() =>
            _gameSystem.RegisterSaveableObject(this);

        public void AddItem(string itemId) =>
            _items.Add(itemId);

        public bool RemoveItem(string itemToRemoveId, int count = 1)
        {
            List<string> itemsToRemove = _items.FindAll(i => i == itemToRemoveId);
            for (int i = 0; i < count; i++)
            {
                _items.Remove(itemsToRemove[i]);
            }
            return itemsToRemove.Count > 0;
        }

        public bool ContainsItem(string itemId, int count) => 
            _items.FindAll(i => i == itemId).Count >= count;

        private bool ValidateData(InventoryData data)
        {
            if (data == null)
            {
                EditorExtentions.GameLogger.AddMessage($"Failed to cast data. Instance name: {SaveableEntityId}, data type: {data}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            if (data.Name != SaveableEntityId)
            {
                EditorExtentions.GameLogger.AddMessage($"Attempted to set data from another game object. Instance name: {SaveableEntityId}, data name: {data.Name}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            return true;
        }

        public object GetData() => new InventoryData()
        {
            Name = SaveableEntityId,
            Items = new List<string>(_items),
        };

        public bool SetData(object data)
        {
            InventoryData dataToSet = data as InventoryData;
            if (!ValidateData(dataToSet))
            {
                return false;
            }
            _items = new List<string>(dataToSet.Items);
            return true;
        }

        public bool SetData(JObject data) => 
            SetData(data.ToObject<InventoryData>());
    }
}
