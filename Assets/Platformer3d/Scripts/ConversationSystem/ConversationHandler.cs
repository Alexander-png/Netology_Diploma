using Platformer3d.GameCore;
using Platformer3d.Interaction;
using Platformer3d.Scriptable.Conversations.Configurations.Phrases;
using Platformer3d.Scriptable.Conversations.Containers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer3d.ConversationSystem
{
	public class ConversationHandler : MonoBehaviour
	{
		[SerializeField]
		private ConversationContainer _conversationContainer;

		private const int NonInPhrase = -1;

		private GameSystem _gameSystem;

		private List<Phrase> _phrases;
		private int _phraseIndex;

		private Phrase CurrentPhrase => _phrases[_phraseIndex];

		public bool InConversation => _phraseIndex != NonInPhrase;

		public void Initialize(GameSystem system)
        {
			_gameSystem = system;
			_phraseIndex = NonInPhrase;
		}

		public void StartConversation(string id)
        {
			EditorExtentions.GameLogger.AddMessage($"Started conversation with id {id}");
			_phrases = _conversationContainer.GetPhrases(id);
			_gameSystem.SetPlayerHandlingEnabled(false);
			_phraseIndex = NonInPhrase;
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

            switch (CurrentPhrase.PhraseType)
            {
                case PhraseType.Common:
					EditorExtentions.GameLogger.AddMessage($"TODO: UI, data: {CurrentPhrase.Data}");
					break;
                case PhraseType.QuestStart:
					_gameSystem.StartQuest(CurrentPhrase.Data);
					ShowNextPhrase();
					break;
                case PhraseType.QuestEnd:
					_gameSystem.EndQuest(CurrentPhrase.Data);
					ShowNextPhrase();
					break;
                case PhraseType.SwitchConversation:
					SwitchConversationOnTarget(CurrentPhrase.Data);
					ShowNextPhrase();
					break;
				case PhraseType.RemoveItem:
					{
						string[] data = CurrentPhrase.Data.Split('$');
						int count = Convert.ToInt32(data[1]);
						_gameSystem.RemoveItemFromPlayer(data[0], count);
						ShowNextPhrase();
						break;
					}
				case PhraseType.AddItem:
                    {
						EditorExtentions.GameLogger.AddMessage("TODO: item fabric", EditorExtentions.GameLogger.LogType.Warning);
						//string[] data = CurrentPhrase.Data.Split('$');
						//int count = Convert.ToInt32(data[1]);
						//_gameSystem.AddItemToPlayer(, count);
						//ShowNextPhrase();
						break;
					}
				case PhraseType.CheckQuestCompleted:
					{
						string[] data = CurrentPhrase.Data.Split('$');
						if (_gameSystem.CheckQuestCompleted(_gameSystem.CurrentTrigger.InteractionTarget, data[0]))
						{
							SwitchConversationOnTarget(data[1], true);
						}
						else
						{
							ShowNextPhrase();
						}
						break;
					}
            }
        }

		private void SwitchConversationOnTarget(string conversationId, bool reload = false)
			=> (_gameSystem.CurrentTrigger.InteractionTarget as ITalkable).SetConversation(conversationId, reload);
    }
}