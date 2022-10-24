using System.Collections.Generic;
using UnityEngine;

namespace Platformer3d.Scriptable.Quests
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Quest configuraiton")]
	public class QuestConfiguraiton : ScriptableObject
	{
		[SerializeField]
		private string _questId;
		[SerializeField]
		private List<QuestStep> _questSteps;

		public string QuestId => _questId;
		public List<QuestStep> QuestSteps => new List<QuestStep>(_questSteps);
    }
}