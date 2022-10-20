using Platformer3d.Interactables.Triggers;
using Platformer3d.LevelEnvironment.Triggers.Interactable;
using UnityEngine;

namespace Platformer3d.Interactables.Elements.Traps
{
	public class StoneStomperHandler : TrapHandler, ISwitchTriggerTarget
	{
        [SerializeField]
        private StomperTrigger[] _stomperTriggers;

        public override bool TrapEnabled
        {
            get => _stomperTriggers.Length != 0 ? _stomperTriggers[0].TrapEnabled : false;
            set
            {
                foreach (var trigger in _stomperTriggers)
                {
                    trigger.TrapEnabled = value;
                }
            }
        }

        public bool IsSwitchedOn
        {
            get => TrapEnabled; 
            set => TrapEnabled = value;
        }

#if UNITY_EDITOR
        [ContextMenu("Find stomper triggers")]
        private void FindTriggers()
        {
            _stomperTriggers = FindObjectsOfType<StomperTrigger>();
        }
#endif
    }
}