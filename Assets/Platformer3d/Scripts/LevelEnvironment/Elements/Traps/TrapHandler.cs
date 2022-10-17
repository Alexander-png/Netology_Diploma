using UnityEngine;

namespace Platformer3d.LevelEnvironment.Elements.Traps
{
	public abstract class TrapHandler : MonoBehaviour
	{
		[SerializeField]
		private bool _enabledByDefault = true;

        private void Start() => Enabled = _enabledByDefault;
        public abstract bool Enabled { get; set; }
	}
}