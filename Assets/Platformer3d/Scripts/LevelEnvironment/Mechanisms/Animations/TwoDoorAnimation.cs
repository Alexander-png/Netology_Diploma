using System.Collections;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Mechanisms.Animations
{
    public class TwoDoorAnimation : MonoBehaviour
    {
        [SerializeField]
        private Transform _leftDoorAxis;
        [SerializeField]
        private Transform _rightDoorAxis;

        [SerializeField]
        private float _animationSpeed;

        [SerializeField]
        private Vector3 _openDoorAxisRotation;
        [SerializeField]
        private Vector3 _closedDoorAxisRotation;

        public float AnimationTime => 
            Vector3.Distance(_closedDoorAxisRotation, _openDoorAxisRotation) / _animationSpeed;

        public void InitState(bool opened)
        {
            _leftDoorAxis.rotation = Quaternion.Euler(-GetTargetRotation(opened));
            _rightDoorAxis.rotation = Quaternion.Euler(GetTargetRotation(opened));
        }

        public void SetOpened(bool opened)
        {
            StartCoroutine(DoorOpen(_leftDoorAxis, Quaternion.Euler(-GetTargetRotation(opened)), opened));
            StartCoroutine(DoorOpen(_rightDoorAxis, Quaternion.Euler(GetTargetRotation(opened)), opened));
        }

        private IEnumerator DoorOpen(Transform doorAxis, Quaternion targetRotation, bool value)
        {
            Quaternion currentRotation = doorAxis.rotation;
            while (Vector3.Distance(currentRotation.eulerAngles, targetRotation.eulerAngles) > 0.01)
            {
                yield return null;
                currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, _animationSpeed * Time.deltaTime);
                doorAxis.rotation = currentRotation;
            }
            doorAxis.rotation = targetRotation;
        }

        private Vector3 GetTargetRotation(bool value) =>
            value ? _openDoorAxisRotation : _closedDoorAxisRotation;
    }
}
