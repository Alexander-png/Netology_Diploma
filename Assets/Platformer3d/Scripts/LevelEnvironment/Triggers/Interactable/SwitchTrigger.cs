using Platformer3d.CharacterSystem.Interactors;
using Platformer3d.LevelEnvironment.Mechanisms.Switchers;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Triggers.Interactable
{
	public class SwitchTrigger : InteractionTrigger
    {
        [SerializeField]
        private bool _isOneoff;
		[SerializeField]
		private Switcher _targetSwitcher;

        public bool WasSwitched { get; private set; }
        public bool IsOneOff => _isOneoff;
        public override bool CanPerform => !(WasSwitched && _isOneoff);

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

        public override void Perform()
        {
            if (CanPerform)
            {
                _targetSwitcher.IsSwitchedOn = !_targetSwitcher.IsSwitchedOn;
                WasSwitched = true;
            }
        }
    }
}