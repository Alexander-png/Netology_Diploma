using Platformer3d.CharacterSystem.Base;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Elements.Usable
{
	public class Portal : MonoBehaviour
	{
        [SerializeField]
        private Vector3 _portalPoint;
		[SerializeField]
		private Portal _pairedPortal;

        public Vector3 MovePoint => transform.position + _portalPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (_pairedPortal != null)
            {
                if (other.TryGetComponent(out Character character))
                {
                    other.transform.position = _pairedPortal.MovePoint;
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color c = Color.cyan;
            c.a = 0.6f;
            Gizmos.color = c;
            Gizmos.DrawSphere(MovePoint, 0.3f);

            if (_pairedPortal != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, _pairedPortal.transform.position);
            }
        }
#endif
    }
}