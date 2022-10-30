using Platformer3d.GameCore;
using Platformer3d.Interaction;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Platformer3d.CharacterSystem.Interactors
{
    public class SwitchTriggerInteractor : MonoBehaviour
    {
        [Inject]
        private GameSystem _gameSystem;
        
        private bool _canInteract;

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

        private void OnInteract(InputValue value)
        {
            if (_canInteract && HandlingEnabled)
            {
                _gameSystem.PerformTrigger();
            }
        }

        private IEnumerator ShowTooltipDelay(float time)
        {
            yield return new WaitForSeconds(time);
            EditorExtentions.GameLogger.AddMessage($"TODO: showing interaction tooltip, current interaction: {CurrentTrigger.ActionId}", EditorExtentions.GameLogger.LogType.Warning);
            _canInteract = true;
        }
    }
}