using UnityEngine;

namespace Platformer3d.LevelEnvironment.Mechanisms.Animations
{
    public class TwoDoorGateAnimation : MonoBehaviour
    {
        [SerializeField]
        private Transform _leftDoorAxis;
        [SerializeField]
        private Transform _rightDoorAxis;

        [SerializeField]
        private Vector3 _openDoorAxisRotation;
    }
}
