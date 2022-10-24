using Platformer3d.Scriptable.Quests;
using System;
using System.Collections.Generic;

namespace Platformer3d.QuestSystem
{
	public class AddRemoveItemQuestStepData
    {
        public QuestStepType Type;
        public string ItemId;
        public int ItemsToComplete;

        public bool Completed => ItemsToComplete == 0;
    }

    // TODO: Current implementation of the quest system only supports find/bring items quests. it's not universal, but i don't know how to implement it better now.
	public class ItemQuest
	{
        private int _stepIndex;

        public string QuestID { get; set; }
		public IQuestGiver QuestGiver { get; set; }
		public List<AddRemoveItemQuestStepData> Steps { get; private set; }

        private AddRemoveItemQuestStepData CurrentStep => Steps[_stepIndex];

        public bool AllStepsCompleted => Steps.TrueForAll(x => x.Completed);

		public void FillQuestSteps(List<QuestStep> stepsToAdd, string[] additionalData)
        {
			Steps = new List<AddRemoveItemQuestStepData>(stepsToAdd.Count);
            for (int i = 0; i < stepsToAdd.Count; i++)
            {
                AddRemoveItemQuestStepData step = new AddRemoveItemQuestStepData()
                {
                    Type = stepsToAdd[i].StepType,
                    ItemId = stepsToAdd[i].Data,
                    ItemsToComplete = Convert.ToInt32(additionalData[i]),
                };
                Steps.Add(step);
            }
        }

        public void OnItemAdded(string itemId, int count = 1)
        {
            foreach (var step in Steps)
            {
                if (step.Type == QuestStepType.FindItem)
                {
                    step.ItemsToComplete -= count;
                }
            }
        }

        public void OnItemRemoved(string itemId, int count = 1)
        {
            foreach (var step in Steps)
            {
                if (step.Type == QuestStepType.FindItem)
                {
                    step.ItemsToComplete += count;
                }
            }
        }
    }
}