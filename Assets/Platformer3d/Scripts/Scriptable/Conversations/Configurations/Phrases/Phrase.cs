using System;

namespace Platformer3d.Scriptable.Conversations.Configurations.Phrases
{
    public enum PhraseType
    {
		Common = 0,
		QuestStart = 1,
		QuestEnd = 2,
		SwitchConversation = 3,
	}

    [Serializable]
	public struct Phrase 
	{
		public PhraseType PhraseType;
		public string Data;
	}
}