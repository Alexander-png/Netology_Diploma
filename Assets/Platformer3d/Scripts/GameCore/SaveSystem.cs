using Newtonsoft.Json;
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
        public object GetData();
        public bool SetData(object data);
    }

    public abstract class SaveData
    {
        public string Name;
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

            private object _data;

            [JsonIgnore]
            public ISaveable SaveableObject => _saveableObject;
            public object Data => _data;

            public SaveDataItem(ISaveable saveableObject)
            {
                _saveableObject = saveableObject;
                _data = saveableObject.GetData();
            }

            public void UpdateData() =>
                _data = _saveableObject.GetData();

            public void RevertData() =>
                _saveableObject.SetData(_data);

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
            RevertRegisteredObjects();
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
            SaveRegisteredObjects();
            if (writeSave)
            {
                WriteSaveFile();
            }
        }

        private void WriteSaveFile()
        {
            PlayerPrefs.SetString(SaveFileName, JsonConvert.SerializeObject(_saveData));
            PlayerPrefs.Save();
        }

        public void LoadLastAutoSave() => LoadLastState();
        private void SaveRegisteredObjects() => _saveData.ForEach(s => s.UpdateData());
        private void RevertRegisteredObjects() => _saveData.ForEach(s => s.RevertData());
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