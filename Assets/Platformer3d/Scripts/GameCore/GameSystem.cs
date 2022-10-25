using Platformer3d.CameraMovementSystem;
using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.DataContainers;
using Platformer3d.ConversationSystem;
using Platformer3d.EditorExtentions;
using Platformer3d.Interaction;
using Platformer3d.PlayerSystem;
using Platformer3d.QuestSystem;
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
        private QuestHandler _questHandler;

        [SerializeField]
        private MovementSkillContainer _playerMovementSkillContainer;

        private PlayerDataContainer _lastPlayerData;

        public InteractionTrigger CurrentTrigger { get; private set; }
        public ConversationHandler ConversationHandler => _conversationHandler;
        public QuestHandler QuestHandler => _questHandler;

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

        public bool CheckQuestCompleted(IPerformer interactionTarget, string questId) =>
            _questHandler.IsQuestCompleted(interactionTarget as IQuestGiver, questId);

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

        public void StartQuest(string questId)
        {
            _questHandler.StartQuest(CurrentTrigger.InteractionTarget as IQuestGiver, questId, _playerCharacter.Inventory.Items);
        }

        public void EndQuest(string questId) =>
            _questHandler.EndQuest(CurrentTrigger.InteractionTarget as IQuestGiver, questId);

        public void OnCollectalbeCollected(IInventoryItem item)
        {
            _questHandler.OnItemAdded(item as IQuestTarget);
            _playerCharacter.Inventory.AddItem(item);
        }

        public void AddItemToPlayer(IInventoryItem item)
        {
            _playerCharacter.Inventory.AddItem(item);
        }

        public void RemoveItemFromPlayer(string itemId, int count = 1) =>
            _playerCharacter.Inventory.RemoveItem(itemId, count);

        public bool CheckItemInInventory(string itemId, int count = 1) => 
            _playerCharacter.Inventory.ContainsItem(itemId, count);

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