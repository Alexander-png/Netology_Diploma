using Platformer3d.CameraMovementSystem;
using Platformer3d.CharacterSystem.Base;
using Platformer3d.ConversationSystem;
using Platformer3d.EditorExtentions;
using Platformer3d.Interaction;
using Platformer3d.PlayerSystem;
using Platformer3d.QuestSystem;
using Platformer3d.Scriptable.Skills.Containers;
using System;
using UnityEngine;

// TODO: 2-3 kinds of enemies
// TODO: find assets for all objects
// TODO: More player abilities
// TODO: UI
// TODO: improve player moving, there are some bugs
// TODO: non movement skills

namespace Platformer3d.GameCore
{
    public class GameSystem : MonoBehaviour
    {
        [SerializeField]
        private Player _playerCharacter;
        [SerializeField]
        private CameraAligner _cameraAligner;

        [SerializeField, Space(15)]
        private ConversationHandler _conversationHandler;
        [SerializeField]
        private QuestHandler _questHandler;
        [SerializeField]
        private SaveSystem _saveSystem;

        [SerializeField, Space(15)]
        private MovementSkillContainer _playerMovementSkillContainer;

        public InteractionTrigger CurrentTrigger { get; private set; }
        public ConversationHandler ConversationHandler => _conversationHandler;
        public QuestHandler QuestHandler => _questHandler;
        public SaveSystem TimelineSystem => _saveSystem;

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
            SetPlayerHandlingEnabled(true);
        }

        private void OnEnable()
        {
            _cameraAligner.ShowAreaExecuted += OnAreaShowed;
        }

        private void OnDisable()
        {
            _cameraAligner.ShowAreaExecuted -= OnAreaShowed;
            StopAllCoroutines();
        }

        public void RegisterSaveableObject(ISaveable saveableObject) =>
            _saveSystem.RegisterSaveableObject(saveableObject);

        public bool CheckQuestCompleted(IPerformer interactionTarget, string questId) =>
            _questHandler.IsQuestCompleted(interactionTarget as IQuestGiver, questId);

        public void OnPlayerRespawned() =>
            PlayerRespawned?.Invoke(this, EventArgs.Empty);

        private void OnAreaShowed(object sender, EventArgs e) =>
            SetPlayerHandlingEnabled(true);

        public void SetPlayerHandlingEnabled(bool value) => _playerCharacter.HandlingEnabled = value;

        public void GiveSkillToPlayer(string abilityId)
        {
            (_playerCharacter as ISkillObservable).SkillObserver.AddSkill(_playerMovementSkillContainer.CreateSkill(abilityId));
            GameLogger.AddMessage($"Given ability with id {abilityId} to player.");
        }

        public void PerformAutoSave(Vector3 checkpointPosition) =>
            _saveSystem.PerformAutoSave(checkpointPosition);

        public void ShowAreaUntilActionEnd(Transform position, Action action, float waitTime)
        {
            _playerCharacter.MovementController.Velocity = Vector3.zero;
            SetPlayerHandlingEnabled(false);
            _cameraAligner.SetFocusPositionUntilActionEnd(position, action, waitTime);
        }

        public Transform GetCameraFocusPosition() =>
            _cameraAligner.FocusPoint;

        public void SetCurrentTrigger(InteractionTrigger trigger) => CurrentTrigger = trigger;

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

        public void StartQuest(string questId) =>
            _questHandler.StartQuest(CurrentTrigger.InteractionTarget as IQuestGiver, questId, _playerCharacter.Inventory.Items);

        public void EndQuest(string questId) =>
            _questHandler.EndQuest(CurrentTrigger.InteractionTarget as IQuestGiver, questId);

        public void OnCollectalbeCollected(IInventoryItem item)
        {
            _questHandler.OnItemAdded(item as IQuestTarget);
            _playerCharacter.Inventory.AddItem(item);
        }

        public void AddItemToPlayer(IInventoryItem item) =>
            _playerCharacter.Inventory.AddItem(item);

        public void RemoveItemFromPlayer(string itemId, int count = 1) =>
            _playerCharacter.Inventory.RemoveItem(itemId, count);

        public bool CheckItemInInventory(string itemId, int count = 1) => 
            _playerCharacter.Inventory.ContainsItem(itemId, count);

        public Player GetPlayer() => _playerCharacter;

#if UNITY_EDITOR
        [ContextMenu("Fill fields")]
		private void FindPlayerOnScene()
        {
			_playerCharacter = FindObjectOfType<Player>();
            _cameraAligner = FindObjectOfType<CameraAligner>();
            _conversationHandler = GetComponent<ConversationHandler>();
            _saveSystem = GetComponent<SaveSystem>();
        }
#endif
    }
}