using Platformer3d.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer3d.CharacterSystem.NPC
{
	public abstract class BaseNPC : MonoBehaviour
	{
		[Inject]
		private GameSystem _gameSystem;
		[SerializeField]
		private string _actionId;

		public GameSystem GameSystem => _gameSystem;
		public string ActionId => _actionId;
	}
}