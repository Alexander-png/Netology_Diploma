using Platformer3d.GameCore;
using Platformer3d.Interaction;
using Platformer3d.PlayerSystem;
using System.Collections;
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
                if (value != null && value.CanPerform)
                {
                    StopAllCoroutines();
                    StartCoroutine(ShowTooltipDelay(value.InteractionDelay));
                }

                if (value == null)
                {
                    _canInteract = false;
                    StopAllCoroutines();
                }
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

        // TODO: remove delay?
        private IEnumerator ShowTooltipDelay(float time)
        {
            yield return new WaitForSeconds(time);
            //_gameSystem.ShowInteractionTooltip();
            _canInteract = true;
        }
    }
}