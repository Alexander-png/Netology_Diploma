using Platformer3d.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer3d.LevelEnvironment.Elements.Common
{
	public class BaseLevelElement : MonoBehaviour
	{
        [Inject]
        private GameSystem _gameSystem;

        public GameSystem GameSystem => _gameSystem;
    }
}