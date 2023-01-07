using Platformer3d.GameCore;
using Platformer3d.PlayerSystem;
using UnityEngine;
using Zenject;

namespace Platformer3d.LevelEnvironment.Triggers
{
    [RequireComponent(typeof(BoxCollider))]
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

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            BoxCollider boxCollider = GetComponent<BoxCollider>();

            Gizmos.color = new Color(0f, 0f, 1f, 0.6f);
            Gizmos.DrawCube(transform.position, boxCollider.size);
        }
#endif
    }
}