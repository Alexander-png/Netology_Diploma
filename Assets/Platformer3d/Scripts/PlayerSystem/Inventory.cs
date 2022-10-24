using System.Collections.Generic;
using UnityEngine;

namespace Platformer3d.PlayerSystem
{
    public interface IInventoryItem 
    {
        public string ItemId { get; }
    }

    public class Inventory : MonoBehaviour
    {
        private List<IInventoryItem> _items;

        private void Awake()
        {
            _items = new List<IInventoryItem>();
        }

        public void AddItem(IInventoryItem toAdd) =>
            _items.Add(toAdd);

        public bool RemoveItem(IInventoryItem toRemove, int count = 1) =>
            RemoveItem(toRemove.ItemId);

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
    }
}
