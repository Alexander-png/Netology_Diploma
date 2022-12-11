using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Platformer3d.GameCore
{
	public enum SceneTypes : byte
    {
		MainMenu = 0,
		Game = 1,
    }

	public static class GameObserver
	{
		private static Dictionary<SceneTypes, string> _sceneTypes;

		public static bool NewGameFlag;

		static GameObserver()
        {
			InitSceneTypes();
		}

		private static void InitSceneTypes()
        {
			_sceneTypes = new Dictionary<SceneTypes, string>()
			{
				{ SceneTypes.MainMenu, "MainMenu" },
				{ SceneTypes.Game, "Game" },
			};
        }

		public static void SwitchScene(SceneTypes sceneType) =>
			SceneManager.LoadScene(_sceneTypes[sceneType]);
	}
}