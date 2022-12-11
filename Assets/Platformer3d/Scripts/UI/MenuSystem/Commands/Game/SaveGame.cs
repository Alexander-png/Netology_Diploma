using Platformer3d.UI.MenuSystem.Commands.Base;

namespace Platformer3d.UI.MenuSystem.Commands.Game
{
	public class SaveGame : MenuCommand
	{
        public override void Execute()
        {
            _gameSystem.SaveSystem.PerformSave(true);
        }
	}
}