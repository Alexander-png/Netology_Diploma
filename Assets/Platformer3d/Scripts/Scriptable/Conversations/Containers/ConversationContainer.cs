using Platformer3d.Scriptable.Conversations.Configurations;
using Platformer3d.Scriptable.Conversations.Configurations.Phrases;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer3d.Scriptable.Conversations.Containers
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Conversation container")]
	public class ConversationContainer : ScriptableObject
	{
		[SerializeField]
		private List<ConversationConfiguration> _conversations;

		public List<Phrase> GetPhrases(string id) => _conversations.Find(x => x.ConversationId == id).Phrases;
	}
}