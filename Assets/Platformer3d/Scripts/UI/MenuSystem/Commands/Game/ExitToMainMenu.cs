using Platformer3d.GameCore;
using Platformer3d.UI.MenuSystem.Commands.Base;

namespace Platformer3d.UI.MenuSystem.Commands.Game
{
	public class ExitToMainMenu : MenuCommand
	{
        public override void Execute()
        {
            GameObserver.SwitchScene(SceneTypes.MainMenu);
        }
	}
}