using Platformer3d.CharacterSystem.Enums;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Base
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField]
        private SideTypes _side;
        [SerializeField]
        private string _name;

        public SideTypes Side => _side;
        public string Name => _name;

        protected virtual void Awake() { }
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }
    }
}
