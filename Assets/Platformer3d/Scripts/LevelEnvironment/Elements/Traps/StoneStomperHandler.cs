using Platformer3d.LevelEnvironment.Triggers;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Elements.Traps
{
	public class StoneStomperHandler : TrapHandler
	{
        [SerializeField]
        private StomperTrigger[] _stomperTriggers;

        public override bool Enabled
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

#if UNITY_EDITOR
        [ContextMenu("Find stomper triggers")]
        private void FindTriggers()
        {
            _stomperTriggers = FindObjectsOfType<StomperTrigger>();
        }
#endif
    }
}