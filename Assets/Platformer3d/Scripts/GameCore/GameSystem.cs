using Platformer3d.CameraMovementSystem;
using Platformer3d.CharacterSystem;
using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.DataContainers;
using Platformer3d.EditorExtentions;
using Platformer3d.Scriptable.Skills.Containers;
using System;
using System.Collections;
using UnityEngine;

namespace Platformer3d.GameCore
{
	public class GameSystem : MonoBehaviour
	{
        [SerializeField]
        private float _respawnTime; // TODO: find better place
		[SerializeField]
		private Player _playerCharacter;
        [SerializeField]
        private CameraAligner _cameraAligner;

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

        private void Start()
        {
            _lastPlayerData = _playerCharacter.GetData() as PlayerDataContainer;
            _playerCharacter.HandlingEnabled = true;
        }

        private void OnEnable()
        {
            _cameraAligner.ShowAreaExecuted += OnAreaShowed;
            _playerCharacter.Died += OnPlayerDiedInternal;
        }

        private void OnDisable()
        {
            _cameraAligner.ShowAreaExecuted -= OnAreaShowed;
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
            yield return new WaitForSeconds(time);
            RespawnPlayer();
        }

        private void OnAreaShowed(object sender, EventArgs e)
        {
            SetPlayerHandlingEnabled(true);
        }

        public void GiveAbilityToPlayer(string abilityId)
        {
            (_playerCharacter as ISkillObservable).SkillObserver.AddSkill(_playerMoventModificatorContainer.CreateSkill(abilityId));
            GameLogger.AddMessage($"Given ability with id {abilityId} to player.");
        }

        public void OnCheckpointReached(Vector3 checkpointPosition)
        {
            _lastPlayerData = _playerCharacter.GetData() as PlayerDataContainer;
            _lastPlayerData.Position = checkpointPosition;
        }

        public void ShowAreaUntilActionEnd(Transform position, Action action, float switchTime)
        {
            SetPlayerHandlingEnabled(false);
            _cameraAligner.ShowAreaUntilActionEnd(position, action, switchTime);
        }   

        public void SetPlayerHandlingEnabled(bool value) => _playerCharacter.HandlingEnabled = value;

#if UNITY_EDITOR
        [ContextMenu("Fill fields")]
		private void FindPlayerOnScene()
        {
			_playerCharacter = FindObjectOfType<Player>();
            _cameraAligner = FindObjectOfType<CameraAligner>();
        }
#endif
    }
}