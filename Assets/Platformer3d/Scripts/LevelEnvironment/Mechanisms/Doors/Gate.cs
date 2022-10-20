using UnityEngine;

namespace Platformer3d.LevelEnvironment.Mechanisms.Doors
{
	public abstract class Gate : MonoBehaviour
	{
		[SerializeField, Tooltip("Notice: if this object is handled by another object, this field will not work.")]
		protected bool _openedByDefault;

		public abstract bool Opened { get; set; }
	}
}