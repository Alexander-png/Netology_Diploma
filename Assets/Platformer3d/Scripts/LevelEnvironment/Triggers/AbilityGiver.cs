using Platformer3d.CharacterSystem;
using Platformer3d.EditorExtentions;
using Platformer3d.Platformer3d.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer3d.Interactables.Triggers
{
    [System.Obsolete]
	public class AbilityGiver : MonoBehaviour
	{
        [Inject]
        private GameSystem _gameSystem;

        [SerializeField]
        private string _abilityId;

        private void Start()
        {
            GameLogger.AddMessage($"{nameof(AbilityGiver)}: i am a placeholder. You should replace me with actual ability give implementation", GameLogger.LogType.Warning);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _gameSystem.GiveAbilityToPlayer(_abilityId);
            }
        }
    }
}