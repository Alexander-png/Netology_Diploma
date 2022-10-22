using UnityEngine;

namespace Platformer3d.Interaction
{
    public interface ISwitcher : IPerformer
    {
        public string ActionId { get; }
        public bool CanPerform { get; }
        public bool IsSwitchedOn { get; set; }
    }

	public class SwitchTrigger : InteractionTrigger
    {
		[SerializeField]
		private GameObject _targetSwitcher;

        private ISwitcher _switcher;

        public override bool CanPerform => _switcher.CanPerform;
        public override string ActionId => _switcher.ActionId;

        private void Start()
        {
            _interactionTarget = _switcher = _targetSwitcher.GetComponent<ISwitcher>();
        }

        public override void Perform() =>
            _switcher.IsSwitchedOn = !_switcher.IsSwitchedOn;
    }
}