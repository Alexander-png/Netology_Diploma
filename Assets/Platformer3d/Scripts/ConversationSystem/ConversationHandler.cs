using Platformer3d.GameCore;
using Platformer3d.Interaction;
using Platformer3d.Scriptable.Conversations.Configurations.Phrases;
using Platformer3d.Scriptable.Conversations.Containers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Zenject;

namespace Platformer3d.ConversationSystem
{
	public class ConversationHandler : MonoBehaviour
	{
		private const int NotInPhrase = -1;

		[Inject]
		private GameSystem _gameSystem;

		[SerializeField]
		private ConversationContainer _conversationContainer;

		private List<Phrase> _phrases;
		private int _phraseIndex = NotInPhrase;

		private Phrase CurrentPhrase => _phrases[_phraseIndex];

		public bool InConversation => _phraseIndex != NotInPhrase;

		public void StartConversation(string id)
        {
			_phrases = _conversationContainer.GetPhrases(id);
			_gameSystem.SetConversationUIEnabled(true);
			_gameSystem.SetPlayerHandlingEnabled(false);
			_phraseIndex = NotInPhrase;
			ShowNextPhrase();
		}

		public void ShowNextPhrase()
        {
			_phraseIndex++;
			if (_phraseIndex > _phrases.Count - 1)
            {
				_phrases = null;
				_phraseIndex = NotInPhrase;
				_gameSystem.SetPlayerHandlingEnabled(true);
				_gameSystem.SetConversationUIEnabled(false);
				return;
            }

            switch (CurrentPhrase.PhraseType)
            {
                case PhraseType.Common:
					_gameSystem.ShowConversationPhrase(CurrentPhrase.Data);
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
                        Regex regex = new Regex(@"\((\b.+)\)[$](.+)");

						Match match = regex.Match(CurrentPhrase.Data);

						string[] data = new string[] { match.Groups[1].Value, match.Groups[2].Value };

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

		private void SwitchConversationOnTarget(string conversationId, bool reload = false) =>
			 (_gameSystem.CurrentTrigger.InteractionTarget as ITalkable).SetConversation(conversationId, reload);
    }
}