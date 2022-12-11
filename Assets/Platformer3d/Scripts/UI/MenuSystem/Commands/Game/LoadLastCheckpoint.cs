using Platformer3d.UI.MenuSystem.Commands.Base;

namespace Platformer3d.UI.MenuSystem.Commands.Game
{
	public class LoadLastCheckpoint : MenuCommand
	{
        public override void Execute()
        {
            _gameSystem.SaveSystem.LoadLastAutoSave();
            _gameSystem.GamePaused = false;
        }
	}
}