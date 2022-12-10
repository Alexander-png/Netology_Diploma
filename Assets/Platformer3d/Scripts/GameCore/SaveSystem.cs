using Platformer3d.CharacterSystem.DataContainers;
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
        public void SetData(object data);
    }

    public abstract class SaveData
    {
        public string Name;
    }

    public class SaveSystem : MonoBehaviour
	{
        [Inject]
        private GameSystem _gameSystem;

        [SerializeField]
		private float _respawnTime;

        private Player _player;
        private List<SaveDataItem> _saveableData;

        private class SaveDataItem
        {
            private ISaveable _saveableObject;
            private object _data;

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
            _saveableData = new List<SaveDataItem>();
        }

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
        }

        private void OnPlayerDiedInternal(object sender, EventArgs e)
        {
            _player.gameObject.SetActive(false);
            StartCoroutine(PlayerRespawnCoroutine(_respawnTime));
        }

        private void RespawnPlayer()
        {
            RevertRegisteredObjects();
            _player.NotifyRespawn();
            _player.gameObject.SetActive(true);
            _gameSystem.InvokePlayerRespawned();
        }

        private IEnumerator PlayerRespawnCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            RespawnPlayer();
        }

        public void PerformAutoSave(Vector3 checkpointPosition)
        {
            SaveRegisteredObjects();
        }

        private void SaveRegisteredObjects() => _saveableData.ForEach(s => s.UpdateData());
        private void RevertRegisteredObjects() => _saveableData.ForEach(s => s.RevertData());
        private bool IsObjectRegistered(ISaveable obj) => _saveableData.Find(data => data.SaveableObject.Equals(obj)) != null;

        public void RegisterSaveableObject(ISaveable saveableObject)
        {
            if (IsObjectRegistered(saveableObject))
            {
                EditorExtentions.GameLogger.AddMessage("", EditorExtentions.GameLogger.LogType.Warning);
                return;
            }
            _saveableData.Add(new SaveDataItem(saveableObject));
        }

        // TODO: think about saving game object id's instead of names
    }
}