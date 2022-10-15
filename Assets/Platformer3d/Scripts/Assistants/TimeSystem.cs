using UnityEngine;

namespace Platformer3d.Assistants
{
	public class TimeSystem : MonoBehaviour
	{
		[SerializeField]
		private float _timeScale = 1;

		public static TimeSystem Instance { get; private set; }

        public float ScaledDeltaTime => _timeScale * Time.deltaTime;

        private void Awake()
        {
            Instance = this;
        }

        public void SetTimeScale(float value) => _timeScale = value;
    }
}