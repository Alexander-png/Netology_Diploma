using Platformer3d.LevelEnvironment.Switchers;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Mechanisms.Lockers
{
    public class Lock : MonoBehaviour, ISwitcherTarget
    {
        public bool IsSwitchedOn 
        {
            get => throw new System.NotImplementedException(); 
            set => throw new System.NotImplementedException();
        }

        public float SwitchTime => throw new System.NotImplementedException();

        public Transform FocusPoint => throw new System.NotImplementedException();
    }
}