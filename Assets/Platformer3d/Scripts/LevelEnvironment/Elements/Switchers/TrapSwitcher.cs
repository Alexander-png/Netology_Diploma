using Platformer3d.Interactables.Elements.Traps;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Elements.Switchers
{
	public class TrapSwitcher : LevelElementSwitcher
	{
        [SerializeField]
        private TrapHandler _trap;

        public override void Switch(bool value) => _trap.TrapEnabled = value;
	}
}