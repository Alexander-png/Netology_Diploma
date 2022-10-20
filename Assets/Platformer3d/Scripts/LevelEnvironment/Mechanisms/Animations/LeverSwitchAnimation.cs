using System.Collections;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Mechanisms.Animations
{
    public class LeverSwitchAnimation : MonoBehaviour
    {
        [SerializeField]
        private Transform _rodAxis;

        [SerializeField]
        private float _animationSpeed;
        [SerializeField]
        private Vector3 _switchedOnAxisRotation;
        [SerializeField]
        private Vector3 _switchedOffAxisRotation;

        public void Switch(bool value)
        {
            StartCoroutine(LeverSwitch(value, _animationSpeed));
        }

        public void InitState(bool isSwitchedOn)
        {
            _rodAxis.rotation = Quaternion.Euler(GetTargetRotation(isSwitchedOn));
        }

        private IEnumerator LeverSwitch(bool value, float speed)
        {
            Quaternion targetRotation = Quaternion.Euler(GetTargetRotation(value));

            Quaternion currentRotation = _rodAxis.rotation;
            while (Vector3.Distance(currentRotation.eulerAngles, targetRotation.eulerAngles) > 0.01)
            {
                yield return null;
                currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, speed * Time.deltaTime);
                _rodAxis.rotation = currentRotation;
            }
            _rodAxis.rotation = targetRotation;
        }

        private Vector3 GetTargetRotation(bool value) =>
            value ? _switchedOnAxisRotation : _switchedOffAxisRotation;
    }
}