using Platformer3d.LevelEnvironment.Switchers;
using Platformer3d.LevelEnvironment.Triggers;
using UnityEngine;

namespace Platformer3d.Interactables.Elements.Traps
{
	public class StoneStomperHandler : TrapHandler, ISwitcherTarget
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

        public float SwitchTime => 0;

        public Transform FocusPoint => transform;

#if UNITY_EDITOR
        [ContextMenu("Find stomper triggers")]
        private void FindTriggers()
        {
            _stomperTriggers = FindObjectsOfType<StomperTrigger>();
        }
#endif
    }
}