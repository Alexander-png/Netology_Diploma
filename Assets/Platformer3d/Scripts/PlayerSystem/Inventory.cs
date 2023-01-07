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
        [Inject]
        private GameSystem _gameSystem;

        private List<IInventoryItem> _items;

        public IEnumerable<IInventoryItem> Items => new List<IInventoryItem>(_items);

        private void Awake() =>
            _items = new List<IInventoryItem>();

        private void Start() =>
            _gameSystem.RegisterSaveableObject(this);

        public void AddItem(IInventoryItem toAdd) =>
            _items.Add(toAdd);

        public bool RemoveItem(IInventoryItem toRemove, int count = 1) =>
            RemoveItem(toRemove.ItemId, count);

        public bool RemoveItem(string itemToRemoveId, int count = 1)
        {
            List<IInventoryItem> itemsToRemove = _items.FindAll(i => i.ItemId == itemToRemoveId);
            for (int i = 0; i < count; i++)
            {
                _items.Remove(itemsToRemove[i]);
            }
            return itemsToRemove.Count > 0;
        }

        public bool ContainsItem(string itemId, int count) => 
            _items.FindAll(i => i.ItemId == itemId).Count >= count;

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
            data["Items"] = JToken.FromObject(Items);
            return data;
        }

        public bool SetData(JObject data)
        {
            if (!ValidateData(data))
            {
                return false;
            }
            _items = new List<IInventoryItem>(data.Value<List<IInventoryItem>>("Items"));
            return true;
        }
    }
}
