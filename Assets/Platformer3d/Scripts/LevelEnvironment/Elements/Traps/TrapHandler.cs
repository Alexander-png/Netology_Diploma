using UnityEngine;

namespace Platformer3d.Interactables.Elements.Traps
{
	public abstract class TrapHandler : MonoBehaviour
	{
		[SerializeField, Tooltip("Notice: if this object is target of switcher, this field will not work.")]
		private bool _enabledByDefault = true;

        private void Awake() => TrapEnabled = _enabledByDefault;
        public abstract bool TrapEnabled { get; set; }
	}
}