using System;

namespace Platformer3d.Scriptable.Conversations.Configurations.Phrases
{
    public enum PhraseType
    {
		Common = 0,
		QuestStart = 1,
		CheckQuestCompleted = 2,
		QuestEnd = 3,
		SwitchConversation = 4,
		RemoveItem = 5,
		AddItem = 6,
	}

    [Serializable]
	public struct Phrase 
	{
		public PhraseType PhraseType;
		public string Data;
	}
}