using Platformer3d.CharacterSystem.Interactors;
using UnityEngine;

namespace Platformer3d.Interaction
{
    public interface IPerformer
    { 

    }

    public abstract class InteractionTrigger : MonoBehaviour
	{
        [SerializeField]
        private float _interactionDelay;

        protected IPerformer _interactionTarget;

        public virtual string ActionId => string.Empty;
		public virtual bool CanPerform { get; } = true;
		public abstract void Perform();

        public IPerformer InteractionTarget => _interactionTarget;
        public float InteractionDelay => _interactionDelay;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out SwitchTriggerInteractor interactor))
            {
                interactor.CurrentTrigger = this;
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out SwitchTriggerInteractor interactor))
            {
                if (interactor.CurrentTrigger == this)
                {
                    interactor.CurrentTrigger = null;
                }
            }
        }
    }
}