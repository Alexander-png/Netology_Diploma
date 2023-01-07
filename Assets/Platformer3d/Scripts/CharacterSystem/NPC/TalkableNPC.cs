using Newtonsoft.Json.Linq;
using Platformer3d.EditorExtentions;
using Platformer3d.GameCore;
using Platformer3d.Interaction;
using Platformer3d.QuestSystem;
using UnityEngine;

namespace Platformer3d.CharacterSystem.NPC
{
	public class TalkableNPC : BaseNPC, ITalkable, IQuestGiver, IQuestTarget, ISaveable
    {
        [SerializeField]
        private string _conversationId;
        [SerializeField]
        private string _npcId;

        public string QuestTargetId => _npcId;

        public string ConversationId 
        {
            get => _conversationId; 
            set => _conversationId = value;
        }

        protected virtual void Start() =>
            GameSystem.RegisterSaveableObject(this);

        public void Talk()
        {
            GameSystem.ConversationHandler.StartConversation(_conversationId);
        }

        public void SetConversation(string id, bool hotReload = false)
        {
            _conversationId = id;
            if (hotReload)
            {
                Talk();
            }
        }

        protected virtual bool ValidateData(JObject data)
        {
            if (data == null)
            {
                GameLogger.AddMessage($"Failed to cast data. Instance name: {gameObject.name}, data type: {data}", GameLogger.LogType.Error);
                return false;
            }
            if (data.Value<string>("Name") != gameObject.name)
            {
                GameLogger.AddMessage($"Attempted to set data from another game object. Instance name: {gameObject.name}, data name: {data.Value<string>("Name")}", GameLogger.LogType.Error);
                return false;
            }
            return true;
        }

        public virtual JObject GetData()
        {
            var data = new JObject();
            data["Name"] = gameObject.name;
            data["ConversationId"] = ConversationId;
            return data;
        }

        public virtual bool SetData(JObject data)
        {
            if (!ValidateData(data))
            {
                return false;
            }
            Reset(data);
            return true;
        }

        protected virtual void Reset(JObject data)
        {
            _conversationId = data.Value<string>("ConversationId");
        }
    }
}