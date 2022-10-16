using UnityEngine;

namespace Platformer3d.GameCore
{
	public class TimeSystem : MonoBehaviour
	{
		[SerializeField]
		private float _gameTimeScale = 1;

		public static TimeSystem Instance { get; private set; }

        public float ScaledGameDeltaTime => _gameTimeScale * Time.deltaTime;
        public float ScaledGameFixedDeltaTime => _gameTimeScale * Time.fixedDeltaTime;

        private void Awake() => Instance = this;
        public void SetGameTimeScale(float value) => _gameTimeScale = value;
    }
}