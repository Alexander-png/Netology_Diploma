using Platformer3d.GameCore;
using Platformer3d.PlayerSystem;
using UnityEngine;
using Zenject;

namespace Platformer3d.LevelEnvironment.Triggers
{
	public class CheckPoint : MonoBehaviour
	{
        [Inject]
        private GameSystem _gameSystem;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Player _))
            {
                _gameSystem.PerformAutoSave();
            }
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 0f, 1f, 0.6f);
            Gizmos.DrawCube(transform.position, new Vector3(1, 2, 1));
        }
    }
}