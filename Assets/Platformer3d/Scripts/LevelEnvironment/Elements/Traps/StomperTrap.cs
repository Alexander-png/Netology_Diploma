using Platformer3d.LevelEnvironment.Elements.Common;
using System.Collections;
using UnityEngine;

namespace Platformer3d.Interactables.Elements.Traps
{
    public class StomperTrap : HazardLevelElement
    {
        [SerializeField]
        private BaseLevelElement _holder;
        [SerializeField]
        private AnimationCurve _moveTrajectory;
        [SerializeField]
        private Vector3 _activatorPoint;
        [SerializeField]
        private Vector3 _deactivatorPoint;
        [SerializeField]
        private float _activationThreshold = 0.5f;

        private bool _inAttack = false;
        private float _stompTime;
        private Vector3 _startPosition;
        
        private void Awake()
        {
            _stompTime = _moveTrajectory.keys[_moveTrajectory.length - 1].time;
            _startPosition = _holder.transform.position;
        }

        public void OnTriggered()
        {
            if (!TrapEnabled || _inAttack)
            {
                return;
            }
            _inAttack = true;
            StartCoroutine(StompCoroutine());
        }

        protected override void ResetState(bool playerDied = false)
        {
            base.ResetState(playerDied);
            StopAllCoroutines();
            _holder.transform.position = _startPosition;
            _inAttack = false;
        }

        private IEnumerator StompCoroutine()
        {
            float frameTime = 0;
            
            Vector3 currentPosition = _startPosition;

            Vector3 actPoint = _activatorPoint + currentPosition;
            Vector3 deactPoint = _deactivatorPoint + currentPosition;

            while (frameTime <= _stompTime)
            {
                if (Vector3.Distance(_holder.transform.position, actPoint) <= _activationThreshold)
                {
                    DamageEnabled = true;
                }
                else if (Vector3.Distance(_holder.transform.position, deactPoint) <= _activationThreshold)
                {
                    DamageEnabled = false;
                }
                float offset = _moveTrajectory.Evaluate(frameTime);
                currentPosition.y = _startPosition.y + offset;
                _holder.transform.position = currentPosition;
                frameTime += Time.deltaTime;
                yield return null;
            }
            _holder.transform.position = _startPosition;
            DamageEnabled = false;
            _inAttack = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 pointerPosition = _activatorPoint + transform.position;
            pointerPosition.z = -1;
            Gizmos.DrawCube(pointerPosition, Vector3.one);
            Gizmos.color = Color.green;
            pointerPosition = _deactivatorPoint + transform.position;
            pointerPosition.z = -1;
            Gizmos.DrawCube(pointerPosition, Vector3.one);
        }
#endif
    }
}