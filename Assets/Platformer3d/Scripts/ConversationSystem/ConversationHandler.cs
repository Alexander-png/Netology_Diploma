using Platformer3d.GameCore;
using Platformer3d.Interaction;
using Platformer3d.Scriptable.Conversations.Configurations.Phrases;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer3d.ConversationSystem
{
	public class ConversationHandler : MonoBehaviour
	{
		private const int NonInPhrase = -1;

		private GameSystem _gameSystem;

		private List<Phrase> _phrases;
		private int _phraseIndex;

		public bool InConversation => _phraseIndex != NonInPhrase;

		public void Initialize(GameSystem system)
        {
			_gameSystem = system;
			_phraseIndex = NonInPhrase;
		}

		public void StartConversation(List<Phrase> phrases)
        {
			_phrases = phrases;
			_gameSystem.SetPlayerHandlingEnabled(false);
			ShowNextPhrase();
		}

		public void ShowNextPhrase()
        {
			_phraseIndex++;
			if (_phraseIndex > _phrases.Count - 1)
            {
				_phrases = null;
				_phraseIndex = NonInPhrase;
				_gameSystem.SetPlayerHandlingEnabled(true);
				EditorExtentions.GameLogger.AddMessage("Conversation ended.");
				return;
            }

            switch (_phrases[_phraseIndex].PhraseType)
            {
                case PhraseType.Common:
					EditorExtentions.GameLogger.AddMessage($"TODO: UI, data: {_phrases[_phraseIndex].Data}");
					break;
                case PhraseType.QuestStart:
					_gameSystem.StartQuest(_phrases[_phraseIndex].Data);
					ShowNextPhrase();
					break;
                case PhraseType.QuestEnd:
					_gameSystem.EndQuest(_phrases[_phraseIndex].Data);
					ShowNextPhrase();
					break;
                case PhraseType.SwitchConversation:
					SwitchConversationOnTarget(_phrases[_phraseIndex].Data);
					ShowNextPhrase();
					break;
            }
        }

		private void SwitchConversationOnTarget(string conversationId)
			=> (_gameSystem.CurrentTrigger.InteractionTarget as ITalkable).SetConversation(conversationId);
    }
}