using Platformer3d.Platformer3d.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer3d.GameCore
{
	public class DependencyInjecter : MonoInstaller
	{
        [SerializeField]
        private GameSystem _gameSystem;

        public override void InstallBindings()
        {
            
        }

#if UNITY_EDITOR
        [ContextMenu("Find references")]
        private void Configure()
        {
            _gameSystem = FindObjectOfType<GameSystem>();
        }
#endif
    }
}