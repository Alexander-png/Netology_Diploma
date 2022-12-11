using Platformer3d.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer3d.UI.MenuSystem.Commands.Base
{
    [System.Serializable]
	public class MenuCommand : MonoBehaviour
	{
		[Inject]
		protected GameSystem _gameSystem;

		public virtual void Execute() { }
	}
}