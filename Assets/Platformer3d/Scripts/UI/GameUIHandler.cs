using Platformer3d.GameCore;
using Platformer3d.UI.MenuSystem;
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
		private RectTransform _statsBar;
		[SerializeField]
		private ConversationWidget _conversationWidget;

		private bool _onConversation;

		private bool GamePaused
		{
			get => _gameSystem.GamePaused;
			set => _gameSystem.GamePaused = value;
		}

        private void Start() =>
			_conversationWidget.gameObject.SetActive(_onConversation);

        private void OnEnable() 
		{
			_gameSystem.PauseStateChanged += OnPauseStateChanged;
            _gameSystem.ConversationUIEnabledChanged += OnConversationUIEnabledChanged;
            _gameSystem.ConversationPhraseChanged += OnConversationPhraseChanged;
		}

        private void OnDisable()
        {
			_gameSystem.PauseStateChanged -= OnPauseStateChanged;
			_gameSystem.ConversationUIEnabledChanged -= OnConversationUIEnabledChanged;
			_gameSystem.ConversationPhraseChanged -= OnConversationPhraseChanged;
		}

        private void OnPauseSwitch(InputValue input) =>
			GamePaused = !GamePaused;

		private void OnPauseStateChanged(object sender, bool value)
        {
			_menuBackground.gameObject.SetActive(value);
			_pauseMenu.gameObject.SetActive(value);
			_statsBar.gameObject.SetActive(!value);
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
			_statsBar.gameObject.SetActive(!_onConversation);
        }
	}
}