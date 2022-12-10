using Platformer3d.UI.MenuSystem.Commands.Base;

namespace Platformer3d.UI.MenuSystem.Commands
{
	public class ExitGame : MenuCommand
	{
        public override void Execute()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}