using Platformer3d.LevelEnvironment.Triggers.Interactable;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer3d.CharacterSystem.Interactors
{
    public class SwitchTriggerInteracor : MonoBehaviour
    {
        [SerializeField]
        private float _interactionDelay;

        private bool _canInteract;
        private InteractionTrigger _currentTrigger;

        public InteractionTrigger CurrentTrigger
        {
            get => _currentTrigger;
            set
            {
                _currentTrigger = value;
                if (value != null && value.CanPerform)
                {
                    StopAllCoroutines();
                    StartCoroutine(ShowTooltipDelay(_interactionDelay));
                }

                if (value == null)
                {
                    _canInteract = false;
                    StopAllCoroutines();
                }
            }
        }

        private void OnInteract(InputValue value)
        {
            if (CurrentTrigger != null && _canInteract)
            {
                CurrentTrigger.Perform();
            }
        }

        private IEnumerator ShowTooltipDelay(float time)
        {
            yield return new WaitForSeconds(time);
            EditorExtentions.GameLogger.AddMessage($"TODO: showing interaction tooltip, current interaction: {_currentTrigger.ActionId}", EditorExtentions.GameLogger.LogType.Warning);
            _canInteract = true;
        }
    }
}