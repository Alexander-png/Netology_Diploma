using Platformer3d.PlayerSystem;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Triggers
{
	public class AnimationTrigger : MonoBehaviour
	{
        [SerializeField]
        private Animator _targetAnimator;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _targetAnimator.SetFloat("Begin", 1f);
            }
        }
    }
}