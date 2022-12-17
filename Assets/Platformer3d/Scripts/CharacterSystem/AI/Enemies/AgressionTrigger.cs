using Platformer3d.PlayerSystem;
using UnityEngine;

namespace Platformer3d.CharacterSystem.AI.Enemies
{
	public class AgressionTrigger : MonoBehaviour
	{
        [SerializeField]
		private DarkBeetle _owner;
        [SerializeField]
        private BoxCollider _agressionTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _owner.OnPlayerNearby();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _owner.OnPlayerRanAway();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color c = Color.gray;
            c.a = 0.3f;
            Gizmos.color = c;

            if (_agressionTrigger != null)
            {
                Vector3 rangeVisualSize = _agressionTrigger.size;
                Gizmos.DrawCube(transform.position + _agressionTrigger.center, rangeVisualSize);
            }
        }
#endif
    }
}