using Platformer3d.GameCore;
using Platformer3d.UI.MenuSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Platformer3d.UI
{
	public class GameUIHandler : MonoBehaviour
	{
		[Inject]
		private GameSystem _gameSystem;

		[SerializeField, Space(15f)]
		private RectTransform _menuBackground;
		[SerializeField]
		private MenuComponent _pauseMenu;
		[SerializeField]
		private RectTransform _healthBar;
		[SerializeField]
		private ConversationWidget _conversationWidget;
		[SerializeField]
		private RectTransform _interactionTooltip;

		private bool _onConversation;

		private bool GamePaused
		{
			get => _gameSystem.GamePaused;
			set => _gameSystem.GamePaused = value;
		}

        private void Start()
        {
			_conversationWidget.gameObject.SetActive(_onConversation);
			_interactionTooltip.gameObject.SetActive(_gameSystem.CurrentTrigger != null);
			
        }

        private void OnEnable() 
		{
			_gameSystem.PauseStateChanged += OnPauseStateChanged;
            _gameSystem.ConversationUIEnabledChanged += OnConversationUIEnabledChanged;
            _gameSystem.ConversationPhraseChanged += OnConversationPhraseChanged;
            _gameSystem.CurrentTriggerChanged += OnCurrentInteractionTriggerChanged;
            _gameSystem.CurrentTriggerPerformed += OnCurrentTriggerPerformed;
		}

        private void OnDisable()
        {
			_gameSystem.PauseStateChanged -= OnPauseStateChanged;
			_gameSystem.ConversationUIEnabledChanged -= OnConversationUIEnabledChanged;
			_gameSystem.ConversationPhraseChanged -= OnConversationPhraseChanged;
			_gameSystem.CurrentTriggerChanged -= OnCurrentInteractionTriggerChanged;
			_gameSystem.CurrentTriggerPerformed -= OnCurrentTriggerPerformed;
		}

        private void OnPauseSwitch(InputValue input) =>
			GamePaused = !GamePaused;

		private void OnPauseStateChanged(object sender, bool value)
        {
			_menuBackground.gameObject.SetActive(value);
			_pauseMenu.gameObject.SetActive(value);
			_healthBar.gameObject.SetActive(!value);
		}

        private void OnConversationPhraseChanged(object sender, string phraseId)
        {
			// TODO: here may be localization system call
			_conversationWidget.SetText(phraseId);
		}

        private void OnConversationUIEnabledChanged(object sender, bool enabled)
        {
			_onConversation = enabled;
			_conversationWidget.gameObject.SetActive(_onConversation);
			_healthBar.gameObject.SetActive(!_onConversation);
			_interactionTooltip.gameObject.SetActive(_gameSystem.CanCurrentTriggerPerformed);
        }

		private void OnCurrentInteractionTriggerChanged(object sender, EventArgs e) =>
			_interactionTooltip.gameObject.SetActive(_gameSystem.CanCurrentTriggerPerformed);

		private void OnCurrentTriggerPerformed(object sender, EventArgs e) =>
			_interactionTooltip.gameObject.SetActive(false);
	}
}