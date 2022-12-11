using Platformer3d.UI.MenuSystem.Commands.Base;

namespace Platformer3d.UI.MenuSystem.Commands.Game
{
	public class ResumeGame : MenuCommand
	{
        public override void Execute()
        {
            _gameSystem.GamePaused = false;
        }
	}
}