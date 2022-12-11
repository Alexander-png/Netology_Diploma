using Platformer3d.GameCore;
using Platformer3d.UI.MenuSystem.Commands.Base;

namespace Platformer3d.UI.MenuSystem.Commands.MainMenu
{
	public class LoadGame : MenuCommand
	{
        public override void Execute()
        {
            GameObserver.NewGameFlag = false;
            GameObserver.SwitchScene(SceneTypes.Game);
        }
	}
}