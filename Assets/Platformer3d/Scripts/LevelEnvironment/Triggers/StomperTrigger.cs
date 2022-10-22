using Platformer3d.Interactables.Elements.Traps;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Triggers
{
    public class StomperTrigger : MonoBehaviour
    {
        [SerializeField]
        private StomperTrap _stone;

        public bool TrapEnabled
        {
            get => _stone.TrapEnabled;
            set => _stone.TrapEnabled = value;
        }

        private void OnTriggerStay(Collider other)
        {
            _stone.OnTriggered();
        }
    }
}