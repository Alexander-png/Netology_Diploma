using Platformer3d.GameCore;
using Platformer3d.UI.MenuSystem.Commands.Base;

namespace Platformer3d.UI.MenuSystem.Commands.MainMenu
{
	public class StartNewGame : MenuCommand
	{
        public override void Execute()
        {
            GameObserver.NewGameFlag = true;
            GameObserver.SwitchScene(SceneTypes.Game);
        }
	}
}