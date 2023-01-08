using Platformer3d.QuestSystem;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Platformer3d.Scriptable.Quests.Containers
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Quest container")]
	public class QuestContainer : ScriptableObject
	{
		[SerializeField]
		private List<QuestConfiguraiton> _quests;

		private QuestConfiguraiton FindQuest(string id) => _quests.Find(q => q.QuestId == id);

		private Regex _itemQuestExpr = new Regex(@"\w+");

		public ItemQuest BuildQuest(IQuestGiver questGiver, string questData)
		{
			ItemQuest toReturn = new ItemQuest();
			toReturn.QuestGiver = questGiver;
            MatchCollection splittedData = _itemQuestExpr.Matches(questData);

			string[] additionalData = new string[splittedData.Count - 1];
			for (int i = 0; i < additionalData.Length; i++)
            {
				additionalData[i] = splittedData[i + 1].Value;
            }

			toReturn.QuestID = questData;// splittedData[0].Value;
			toReturn.FillQuestSteps(FindQuest(splittedData[0].Value).QuestSteps, additionalData);
			return toReturn;
		}
	}
}