using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Platformer3d.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Platformer3d.GameCore
{
    public interface ISaveable
    {
        public JObject GetData();
        public bool SetData(JObject data);
    }

    public class SaveSystem : MonoBehaviour
	{
        private string SaveFileName = "Save01";

        [Inject]
        private GameSystem _gameSystem;

        [SerializeField]
		private float _respawnTime;

        private Player _player;
        private List<SaveDataItem> _saveData;

        private class SaveDataItem
        {
            [JsonIgnore]
            private ISaveable _saveableObject;
            [JsonIgnore]
            public ISaveable SaveableObject => _saveableObject;

            //private JObject _data;
            public JObject Data;// => _data;

            public SaveDataItem() { }

            public SaveDataItem(ISaveable saveableObject)
            {
                _saveableObject = saveableObject;
                Data = saveableObject.GetData();
            }

            public void UpdateData()
            {
                Data = _saveableObject.GetData();
            }

            public void RevertData() =>
                _saveableObject.SetData(Data);

            public bool IsTheSameObject(ISaveable obj) => obj == _saveableObject;
        }

        private void Awake()
        {
            _saveData = new List<SaveDataItem>();
        }

        private void OnEnable()
        {
            InitializeInternals();
            _gameSystem.GameLoaded += OnGameLoaded;
        }

        private void OnDisable()
        {
            if (_player != null)
            {
                _player.Died -= OnPlayerDiedInternal;
            }
            _gameSystem.GameLoaded -= OnGameLoaded;
        }

        private void InitializeInternals()
        {
            _player = _gameSystem.GetPlayer();
            _player.Died += OnPlayerDiedInternal;
        }

        private void OnGameLoaded(object sender, EventArgs e)
        {
            if (!GameObserver.NewGameFlag)
            {
                LoadSavedData();
            }
        }

        private void LoadSavedData()
        {
            string data = PlayerPrefs.GetString(SaveFileName, string.Empty);
            if (data == string.Empty)
            {
                return;
            }
            try
            {
                var dataList = JsonConvert.DeserializeObject<List<SaveDataItem>>(data);
                EditorExtentions.GameLogger.AddMessage($"Game loaded successfully");
            }
            catch (Exception exc)
            {
                EditorExtentions.GameLogger.AddMessage($"TODO: fix problem with savefile load. Error: {exc.Message}");
            }
        }

        private void OnPlayerDiedInternal(object sender, EventArgs e)
        {
            _player.gameObject.SetActive(false);
            StartCoroutine(ResetGameStateCoroutine(_respawnTime));
        }

        private void LoadLastState()
        {
            RevertObjects();
            _player.NotifyRespawn();
            _player.gameObject.SetActive(true);
            _gameSystem.InvokePlayerRespawned();
        }

        private IEnumerator ResetGameStateCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            LoadLastState();
        }

        public void PerformSave(bool writeSave = false)
        {
            SaveObjects();
            if (writeSave)
            {
                WriteSaveFile();
                EditorExtentions.GameLogger.AddMessage("Game was saved");
            }
        }

        private void WriteSaveFile()
        {
            PlayerPrefs.SetString(SaveFileName, JsonConvert.SerializeObject(_saveData));
            PlayerPrefs.Save();
        }

        public void LoadLastAutoSave() => LoadLastState();
        private void SaveObjects() => _saveData.ForEach(s => s.UpdateData());
        private void RevertObjects() => _saveData.ForEach(s => s.RevertData());
        private bool IsObjectRegistered(ISaveable obj) => _saveData.Find(data => data.SaveableObject.Equals(obj)) != null;

        public void RegisterSaveableObject(ISaveable saveableObject)
        {
            if (IsObjectRegistered(saveableObject))
            {
                EditorExtentions.GameLogger.AddMessage("", EditorExtentions.GameLogger.LogType.Warning);
                return;
            }
            _saveData.Add(new SaveDataItem(saveableObject));
        }
    }
}