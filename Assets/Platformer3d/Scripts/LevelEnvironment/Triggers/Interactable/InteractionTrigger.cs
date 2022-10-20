using UnityEngine;

namespace Platformer3d.LevelEnvironment.Triggers.Interactable
{
	public abstract class InteractionTrigger : MonoBehaviour
	{
		public virtual string ActionId => string.Empty;

		public virtual bool CanPerform { get; } = true;

		public abstract void Perform();
	}
}