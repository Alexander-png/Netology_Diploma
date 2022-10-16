using Platformer3d.CharacterSystem;
using Platformer3d.EditorExtentions;
using Platformer3d.GameCore;
using System;
using System.Collections;
using UnityEngine;

namespace Platformer3d.Platformer3d.GameCore
{
	public class GameSystem : MonoBehaviour
	{
        [SerializeField]
        private float _respawnTime;
		[SerializeField]
		private Player _playerCharacter;

        private Vector3 _lastPlayerPosition;

        private void Awake()
        {
            if (_playerCharacter == null)
            {
                GameLogger.AddMessage($"{nameof(GameSystem)}: no player character assigned!", GameLogger.LogType.Fatal);
            }
        }

        private void OnEnable()
        {
            _playerCharacter.OnDied += OnPlayerDied;
            RememberPlayerPosition();
        }

        private void RememberPlayerPosition()
        {
            _lastPlayerPosition = _playerCharacter.transform.position;
        }

        private void OnDisable()
        {
            _playerCharacter.OnDied -= OnPlayerDied;

            StopAllCoroutines();
        }

        private void OnPlayerDied(object sender, EventArgs e)
        {
            _playerCharacter.gameObject.SetActive(false);
            StartCoroutine(PlayerRespawnCoroutine(_respawnTime));
        }

        private IEnumerator PlayerRespawnCoroutine(float time)
        {
            while (time > 0)
            {
                yield return null;
                time -= TimeSystem.Instance.ScaledGameDeltaTime;
            }
            RespawnPlayer();
        }

        private void RespawnPlayer()
        {
            _playerCharacter.transform.position = _lastPlayerPosition;
            _playerCharacter.gameObject.SetActive(true);
            RememberPlayerPosition();
        }

        [ContextMenu("Find player in scene")]
		private void FindPlayerOnScene()
        {
			_playerCharacter = FindObjectOfType<Player>();
        }
	}
}