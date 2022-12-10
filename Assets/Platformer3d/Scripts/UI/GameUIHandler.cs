using Platformer3d.UI.MenuSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer3d.UI
{
	public class GameUIHandler : MonoBehaviour
	{
        [SerializeField]
		private bool _gamePaused;

		[SerializeField, Space(15f)]
		private RectTransform _menuBackground;
        [SerializeField]
		private MenuComponent _pauseMenu;

		private bool GamePaused
        {
			get => _gamePaused;
			set
            {
				_gamePaused = value;
				OnPauseStateChanged(value);
			}
		}

        private void Start()
        {
			GamePaused = _gamePaused;
        }

        private void OnPauseSwitch(InputValue input) 
		{
			GamePaused = !GamePaused;
		}

		private void OnPauseStateChanged(bool value)
        {
			_menuBackground.gameObject.SetActive(value);
			_pauseMenu.gameObject.SetActive(value);
		}
	}
}