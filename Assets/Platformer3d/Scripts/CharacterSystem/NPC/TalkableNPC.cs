using Platformer3d.Interaction;
using UnityEngine;

namespace Platformer3d.CharacterSystem.NPC
{
	public class TalkableNPC : BaseNPC, ITalkable
    {
        [SerializeField]
        private string _conversationId;

        public string ConversationId 
        {
            get => _conversationId; 
            set => _conversationId = value;
        }

        public void Talk()
        {
            GameSystem.StartConversation(_conversationId);
        }

        public void SetConversation(string id)
        {
            _conversationId = id;
        }
    }
}