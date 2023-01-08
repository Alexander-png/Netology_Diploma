using Platformer3d.PlayerSystem;
using Platformer3d.Scriptable.Quests.Containers;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer3d.QuestSystem
{
	public class QuestHandler : MonoBehaviour
	{
        [SerializeField]
        private QuestContainer _questContainer;

        private List<ItemQuest> _currentQuests;
        private ItemQuest FindQuest(string id) => _currentQuests.Find(q => q.QuestID == id);
        private ItemQuest FindQuest(string id, IQuestGiver giver) => _currentQuests.Find(q => q.QuestID == id && q.QuestGiver == giver);

        private void Start()
        {
            _currentQuests = new List<ItemQuest>();
        }

        public void StartQuest(IQuestGiver questGiver, string questId, IEnumerable<string> itemsInInventory)
        {
            EditorExtentions.GameLogger.AddMessage($"TODO: quest start, data: {questId}");
            var newQuest = _questContainer.BuildQuest(questGiver, questId);
            _currentQuests.Add(newQuest);
            foreach (var item in itemsInInventory)
            {
                newQuest.OnItemAdded(item);
            }
        }

        public void OnItemAdded(IQuestTarget itemId)
        {
            if (itemId == null)
            {
                return;
            }

            _currentQuests.ForEach(x =>
            {
                x.OnItemAdded(itemId.QuestTargetId);
            });
        }

        public void OnItemRemoved(string itemId)
        {
            if (itemId == null)
            {
                return;
            }

            _currentQuests.ForEach(x =>
            {
                x.OnItemRemoved(itemId);
            });
        }

        public bool IsQuestCompleted(IQuestGiver questGiver, string questId)
        {
            var questToCheck = FindQuest(questId, questGiver);
            if (questToCheck == null)
            {
                return false;
            }
            return questToCheck.AllStepsCompleted;
        }

        public void EndQuest(IQuestGiver questGiver, string questId)
        {
            var questToRemove = FindQuest(questId, questGiver);

            if (questToRemove != null)
            {
                _currentQuests.Remove(questToRemove);
                EditorExtentions.GameLogger.AddMessage($"Quest ended. Data - id: {questId}; giver id: {questGiver}");
            }
            else
            {
                EditorExtentions.GameLogger.AddMessage($"Quest was not found. Data - id: {questId}; giver id: {questGiver}", EditorExtentions.GameLogger.LogType.Warning);
            }
        }
    }
}