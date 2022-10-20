using Platformer3d.CharacterSystem.Interactors;
using Platformer3d.LevelEnvironment.Mechanisms.Switchers;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Triggers.Interactable
{
	public class SwitchTrigger : InteractionTrigger
    {
		[SerializeField]
		private Switcher _targetSwitcher;

        public override bool CanPerform => _targetSwitcher.CanPerform;
        public override string ActionId => _targetSwitcher.ActionId;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out SwitchTriggerInteractor interactor))
            {
                interactor.CurrentTrigger = this;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out SwitchTriggerInteractor interactor))
            {
                if (interactor.CurrentTrigger == this)
                {
                    interactor.CurrentTrigger = null;
                }
            }
        }

        public override void Perform() =>
            _targetSwitcher.IsSwitchedOn = !_targetSwitcher.IsSwitchedOn;
    }
}