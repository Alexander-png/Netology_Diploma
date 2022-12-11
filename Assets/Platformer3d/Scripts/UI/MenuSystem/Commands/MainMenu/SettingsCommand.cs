using Platformer3d.UI.MenuSystem.Commands.Base;

namespace Platformer3d.UI.MenuSystem.Commands.MainMenu
{
	public class SettingsCommand : MenuCommand
	{
        public override void Execute()
        {
            EditorExtentions.GameLogger.AddMessage("Todo: settings");
        }
	}
}