using Platformer3d.PlayerSystem;
using UnityEngine;

namespace Platformer3d.ActivitySystem.Escape
{
	public class EscapeActivityStage : MonoBehaviour
	{
        [SerializeField]
        private float _hazardSpeed;

        private HazardEscapeObserver _observer;

        public Vector3 Position => transform.position;
        public float HazardSpeed => _hazardSpeed;

        public void Init(HazardEscapeObserver activityObserver)
        {
            _observer = activityObserver;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                _observer.OnStagePassedByPlayer(this);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }
}