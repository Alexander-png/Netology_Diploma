using Newtonsoft.Json.Linq;
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

        protected class TalkableNPCData : SaveData
        {
            public string ConversationId;
        }

        protected virtual void Start()
        {
            GameSystem.RegisterSaveableObject(this);
        }

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

        protected virtual bool ValidateData(TalkableNPCData data)
        {
            if (data == null)
            {
                EditorExtentions.GameLogger.AddMessage($"Failed to cast data. Instance name: {gameObject.name}, data type: {data}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            if (data.Name != gameObject.name)
            {
                EditorExtentions.GameLogger.AddMessage($"Attempted to set data from another game object. Instance name: {gameObject.name}, data name: {data.Name}", EditorExtentions.GameLogger.LogType.Error);
                return false;
            }
            return true;
        }

        public virtual object GetData() => new TalkableNPCData() 
        {
            Name = gameObject.name, 
            ConversationId = _conversationId 
        };

        public virtual bool SetData(object data)
        {
            TalkableNPCData dataToSet = data as TalkableNPCData;

            if (!ValidateData(dataToSet))
            {
                return false;
            }
            Reset(dataToSet);
            return true;
        }

        public virtual bool SetData(JObject data) => 
            SetData(data.ToObject<TalkableNPCData>());

        protected virtual void Reset(TalkableNPCData data)
        {
            _conversationId = data.ConversationId;
        }
    }
}