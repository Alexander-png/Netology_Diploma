using Platformer3d.GameCore;
using Platformer3d.PlayerSystem;
using Platformer3d.QuestSystem;
using UnityEngine;
using Zenject;

namespace Platformer3d.LevelEnvironment.Collectables
{
	public class CollectableItem : MonoBehaviour, IInventoryItem, IQuestTarget
    {
		[Inject]
		private GameSystem _gameSystem;

		[SerializeField]
		private string _itemId;

        public string QuestTargetId => _itemId;
        public string ItemId => _itemId;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _gameSystem.OnCollectalbeCollected(this);
                transform.gameObject.SetActive(false);
            }
        }
    }
}