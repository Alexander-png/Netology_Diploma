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

		private bool GamePaused
        {
			get => _gameSystem.GamePaused;
			set => _gameSystem.GamePaused = value;
		}

        private void OnEnable() =>
			_gameSystem.PauseStateChanged += OnPauseStateChanged;

        private void OnDisable() =>
			_gameSystem.PauseStateChanged -= OnPauseStateChanged;

        private void OnPauseSwitch(InputValue input) =>
			GamePaused = !GamePaused;

		private void OnPauseStateChanged(object sender, bool value)
        {
			_menuBackground.gameObject.SetActive(value);
			_pauseMenu.gameObject.SetActive(value);
			_statsBar.gameObject.SetActive(!value);
		}
	}
}