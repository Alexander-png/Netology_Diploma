using Platformer3d.CameraMovementSystem;
using Platformer3d.CharacterSystem.Base;
using Platformer3d.ConversationSystem;
using Platformer3d.EditorExtentions;
using Platformer3d.Interaction;
using Platformer3d.PlayerSystem;
using Platformer3d.QuestSystem;
using Platformer3d.Scriptable.Skills.Containers;
using System;
using System.Collections;
using UnityEngine;

// TODO: find assets for all objects
// TODO: non movement skills
// TODO: think about saving game object id's instead of names
// TODO: rework level layout
// TODO: improve player moving, there are some bugs
// TODO: think about making interactable triggers one off instead of levers

namespace Platformer3d.GameCore
{
    public class GameSystem : MonoBehaviour
    {
        [SerializeField]
        private bool _gamePaused;

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

        private bool _isGameCompleted;
        private InteractionTrigger _currentTrigger;

        public bool GamePaused
        {
            get => _gamePaused;
            set
            {
                _gamePaused = value;
                PauseStateChanged?.Invoke(this, value);
            }
        }

        public InteractionTrigger CurrentTrigger
        {
            get => _currentTrigger;
            private set
            {
                _currentTrigger = value;
                CurrentTriggerChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool CanCurrentTriggerPerformed => CurrentTrigger != null && CurrentTrigger.CanPerform;
        public ConversationHandler ConversationHandler => _conversationHandler;
        public QuestHandler QuestHandler => _questHandler;
        public SaveSystem SaveSystem => _saveSystem;
        public bool IsGameCompleted => _isGameCompleted;

        public event EventHandler PlayerRespawned;
        public event EventHandler<bool> PauseStateChanged;
        public event EventHandler GameLoaded;
        public event EventHandler<bool> ConversationUIEnabledChanged;
        public event EventHandler<string> ConversationPhraseChanged;
        public event EventHandler CurrentTriggerChanged;
        public event EventHandler CurrentTriggerPerformed;
        public event EventHandler GameCompleted;


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
            GamePaused = GamePaused;
            StartCoroutine(LoadedNotifier());
        }

        private IEnumerator LoadedNotifier()
        {
            yield return null;
            GameLoaded?.Invoke(this, EventArgs.Empty);
        }

        private void OnPauseStateChanged(object sender, bool e)
        {
            Time.timeScale = _gamePaused ? 0f : 1f;
        }

        private void OnEnable()
        {
            _cameraAligner.ShowAreaExecuted += OnAreaShowed;
            PauseStateChanged += OnPauseStateChanged;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _cameraAligner.ShowAreaExecuted -= OnAreaShowed;
            PauseStateChanged -= OnPauseStateChanged;
        }

        public void RegisterSaveableObject(ISaveable saveableObject) =>
            _saveSystem.RegisterSaveableObject(saveableObject);

        public bool CheckQuestCompleted(IPerformer interactionTarget, string questId) =>
            _questHandler.IsQuestCompleted(interactionTarget as IQuestGiver, questId);

        public void InvokePlayerRespawned() =>
            PlayerRespawned?.Invoke(this, EventArgs.Empty);

        private void OnAreaShowed(object sender, EventArgs e) =>
            SetPlayerHandlingEnabled(true);

        public void SetPlayerHandlingEnabled(bool value) => _playerCharacter.HandlingEnabled = value;

        public void GiveSkillToPlayer(string abilityId)
        {
            (_playerCharacter as ISkillObservable).SkillObserver.AddSkill(_playerMovementSkillContainer.CreateSkill(abilityId));
            GameLogger.AddMessage($"Given ability with id {abilityId} to player.");
        }

        public bool CheckSkillAdded(string skillId) =>
            (_playerCharacter as ISkillObservable).SkillObserver.CheckSkillAdded(skillId);

        public void PerformAutoSave() =>
            _saveSystem.PerformSave();

        public void ShowAreaUntilActionEnd(Transform position, Action action, float waitTime)
        {
            // TODO: notify player about beginning of cutscene instead of manipulating him directly
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
                CurrentTriggerPerformed?.Invoke(this, EventArgs.Empty);
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

        public void SetConversationUIEnabled(bool value) =>
            ConversationUIEnabledChanged?.Invoke(this, value);

        public void ShowConversationPhrase(string phraseId) =>
            ConversationPhraseChanged?.Invoke(this, phraseId);

        public void NotifyGameCompleted()
        {
            _isGameCompleted = true;
            SetPlayerHandlingEnabled(false);
            _playerCharacter.MovementController.Velocity = Vector3.zero;
            GameCompleted?.Invoke(this, EventArgs.Empty);
        }

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