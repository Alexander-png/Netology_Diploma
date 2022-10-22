using Platformer3d.CameraMovementSystem;
using Platformer3d.CharacterSystem;
using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.DataContainers;
using Platformer3d.ConversationSystem;
using Platformer3d.EditorExtentions;
using Platformer3d.Interaction;
using Platformer3d.Scriptable.Conversations.Containers;
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

        [SerializeField, Space(15)]
        private ConversationHandler _conversationHandler;

        [SerializeField]
        private MovementSkillContainer _playerMovementSkillContainer;
        [SerializeField]
        private ConversationContainer _conversationContainer;

        public InteractionTrigger CurrentTrigger { get; private set; }

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
            _conversationHandler.Initialize(this);
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

        public void SetPlayerHandlingEnabled(bool value) => _playerCharacter.HandlingEnabled = value;

        public void GiveSkillToPlayer(string abilityId)
        {
            (_playerCharacter as ISkillObservable).SkillObserver.AddSkill(_playerMovementSkillContainer.CreateSkill(abilityId));
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

        public void SetTrigger(InteractionTrigger trigger) => CurrentTrigger = trigger;

        public void PerformTrigger()
        {
            if (_conversationHandler.InConversation)
            {
                _conversationHandler.ShowNextPhrase();
            }
            else
            {
                CurrentTrigger.Perform();
            }
        }

        public void StartConversation(string conversationId)
        {
            GameLogger.AddMessage($"Started conversation with id {conversationId}");
            _conversationHandler.StartConversation(_conversationContainer.GetPhrases(conversationId));
        }

        public void StartQuest(string questId)
        {
            GameLogger.AddMessage($"TODO: quest start, data: {questId}");
        }

        public void EndQuest(string questId)
        {
            GameLogger.AddMessage($"TODO: quest end, data: {questId}");
        }

#if UNITY_EDITOR
        [ContextMenu("Fill fields")]
		private void FindPlayerOnScene()
        {
			_playerCharacter = FindObjectOfType<Player>();
            _cameraAligner = FindObjectOfType<CameraAligner>();
            _conversationHandler = GetComponent<ConversationHandler>();
        }
#endif
    }
}