using Platformer3d.Platformer3d.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer3d.LevelEnvironment.Elements.Base
{
	public class BaseLevelElement : MonoBehaviour
	{
        [Inject]
        private GameSystem _gameSystem;

        public GameSystem GameSystem => _gameSystem;
    }
}