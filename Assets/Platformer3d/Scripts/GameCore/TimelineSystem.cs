using Platformer3d.CharacterSystem.DataContainers;
using Platformer3d.PlayerSystem;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Platformer3d.GameCore
{
	public class TimelineSystem : MonoBehaviour
	{
        [Inject]
        private GameSystem _gameSystem;

        [SerializeField]
		private float _respawnTime;

		private PlayerDataContainer _lastPlayerData;

        private Player _player;

        private void OnEnable()
        {
            InitializeInternals();
        }

        private void OnDisable()
        {
            if (_player != null)
            {
                _player.Died -= OnPlayerDiedInternal;
            }
        }

        private void InitializeInternals()
        {
            _player = _gameSystem.GetPlayer();
            _player.Died += OnPlayerDiedInternal;
            _lastPlayerData = _player.GetData() as PlayerDataContainer;
        }

        private void OnPlayerDiedInternal(object sender, EventArgs e)
        {
            _player.gameObject.SetActive(false);
            StartCoroutine(PlayerRespawnCoroutine(_respawnTime));
        }

        private void RespawnPlayer()
        {
            _player.OnRespawn(_lastPlayerData);
            _player.gameObject.SetActive(true);
            _gameSystem.OnPlayerRespawned();
        }

        private IEnumerator PlayerRespawnCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            RespawnPlayer();
        }

        public void PerformAutoSave(Vector3 checkpointPosition)
        {
            _lastPlayerData = _player.GetData() as PlayerDataContainer;
            _lastPlayerData.Position = _player.transform.position;
        }
    }
}