using Platformer3d.Scriptable.Conversations.Configurations.Phrases;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer3d.Scriptable.Conversations.Configurations
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Conversation configuration")]
	public class ConversationConfiguration : ScriptableObject
	{
        [SerializeField]
		private string _conversationId;
		[SerializeField]
		private List<Phrase> _phrases;
		[SerializeField]
		private bool _isLoop;

		public string ConversationId => _conversationId;
		public List<Phrase> Phrases => new List<Phrase>(_phrases);
		public bool IsLoop => _isLoop;
	}
}