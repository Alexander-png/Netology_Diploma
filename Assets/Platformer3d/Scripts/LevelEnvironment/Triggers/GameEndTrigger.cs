using Platformer3d.GameCore;
using Platformer3d.PlayerSystem;
using UnityEngine;
using Zenject;

namespace Platformer3d.LevelEnvironment.Triggers
{
    [RequireComponent(typeof(BoxCollider))]
	public class GameEndTrigger : MonoBehaviour
	{
		[Inject]
		private GameSystem _gameSystem;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Player _))
            {
                _gameSystem.NotifyGameCompleted();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            BoxCollider collider = GetComponent<BoxCollider>();

            Gizmos.color = new Color(0f, 0f, 1f, 0.6f);
            Gizmos.DrawCube(transform.position, collider.size);
        }
#endif

    }
}