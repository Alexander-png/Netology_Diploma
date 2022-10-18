using UnityEngine;

namespace Platformer3d.LevelEnvironment.Triggers.Interactable
{
	public abstract class InteractionTrigger : MonoBehaviour
	{
		[SerializeField]
		private string _actionId;

		public string ActionId => _actionId;

		public virtual bool CanPerform { get; } = true;

		public abstract void Perform();
	}
}