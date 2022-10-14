using UnityEngine;

namespace Platformer3d.UI.MenuSystem.Commands.Base
{
    [System.Serializable]
	public class MenuCommand : MonoBehaviour
	{
		public virtual void Execute() { }
	}
}