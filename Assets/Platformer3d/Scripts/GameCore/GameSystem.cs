using Platformer3d.CharacterSystem;
using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.DataContainers;
using Platformer3d.EditorExtentions;
using Platformer3d.GameCore;
using Platformer3d.Scriptable.Skills.Containers;
using Platformer3d.SkillSystem.Skills;
using System;
using System.Collections;
using UnityEngine;

namespace Platformer3d.Platformer3d.GameCore
{
	public class GameSystem : MonoBehaviour
	{
        [SerializeField]
        private float _respawnTime; // TODO: find better place
		[SerializeField]
		private Player _playerCharacter;

        [SerializeField]
        private MovementSkillContainer _playerMoventModificatorContainer;

        private PlayerDataContainer _lastPlayerData;

        public event EventHandler PlayerRespawned;

        private void Awake()
        {
            if (_playerCharacter == null)
            {
                GameLogger.AddMessage($"{nameof(GameSystem)}: no player character assigned!", GameLogger.LogType.Fatal);
            }
        }

        public void GiveAbilityToPlayer(string abilityId)
        {
            (_playerCharacter as ISkillObservable).SkillObserver.AddSkill(_playerMoventModificatorContainer.CreateSkill(abilityId));
            GameLogger.AddMessage($"Given ability with id {abilityId} to player.");
        }

        private void Start()
        {
            _lastPlayerData = _playerCharacter.GetData() as PlayerDataContainer;
        }

        private void OnEnable()
        {
            _playerCharacter.Died += OnPlayerDiedInternal;
        }

        public void OnCheckpointReached(Vector3 checkpointPosition)
        {
            _lastPlayerData = _playerCharacter.GetData() as PlayerDataContainer;
            _lastPlayerData.Position = checkpointPosition;
        }

        private void OnDisable()
        {
            _playerCharacter.Died -= OnPlayerDiedInternal;
            StopAllCoroutines();
        }

        private void OnPlayerDiedInternal(object sender, EventArgs e)
        {
            _playerCharacter.gameObject.SetActive(false);
            StartCoroutine(PlayerRespawnCoroutine(_respawnTime));
        }
                
        private void RespawnPlayer()
        {
            _playerCharacter.OnRespawn(_lastPlayerData);
            _playerCharacter.gameObject.SetActive(true);
            PlayerRespawned?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerator PlayerRespawnCoroutine(float time)
        {
            //while (time > 0)
            //{
            //    yield return null;
            //    time -= TimeSystem.Instance.ScaledGameDeltaTime;
            //}
            yield return new WaitForSeconds(time);
            RespawnPlayer();
        }

#if UNITY_EDITOR
        [ContextMenu("Find player in scene")]
		private void FindPlayerOnScene()
        {
			_playerCharacter = FindObjectOfType<Player>();
        }
#endif
    }
}