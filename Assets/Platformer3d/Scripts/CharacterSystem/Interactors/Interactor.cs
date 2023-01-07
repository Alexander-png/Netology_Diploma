using Platformer3d.GameCore;
using Platformer3d.Interaction;
using Platformer3d.PlayerSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Platformer3d.CharacterSystem.Interactors
{
    public class Interactor : MonoBehaviour
    {
        [Inject]
        private GameSystem _gameSystem;
        
        private bool _canInteract;
        private Player _player;

        protected virtual void Start()
        {
            _player = _gameSystem.GetPlayer();
        }

        public InteractionTrigger CurrentTrigger
        {
            get => _gameSystem.CurrentTrigger;
            set
            {
                _gameSystem.SetCurrentTrigger(value);
                _canInteract = _gameSystem.CanCurrentTriggerPerformed;
            }
        }

        public bool HandlingEnabled { get; set; } = true;

        public void OnInteractPerformed(InputValue value)
        {
            if (_canInteract && HandlingEnabled)
            {
                if (CurrentTrigger.NeedStop)
                {
                    _player.MovementController.Velocity = Vector3.zero;
                }
                _gameSystem.PerformTrigger();
            }
        }
    }
}