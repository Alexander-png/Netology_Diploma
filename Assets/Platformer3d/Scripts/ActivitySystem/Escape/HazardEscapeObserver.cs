using Platformer3d.Interactables.Elements.Traps;
using Platformer3d.PlayerSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

// TODO: change player death conditions in this activity

namespace Platformer3d.ActivitySystem.Escape
{
    public class HazardEscapeObserver : ActivityObserver
	{
		[SerializeField]
		private HazardLevelElement _hazard;
        [SerializeField]
        private Transform _hazardCameraFocusPoint;

        [SerializeField]
        private List<EscapeActivityStage> _stages;

        private Player _player;
        private int _pointIndex = -1;
        private Vector3 _hazardInitialPosition;

        private bool CanStart => !_inAction || !(IsOneOff && _activityEnded);
        private bool IsLastStage => _stages != null ? _pointIndex >= _stages.Count - 1 : false;

        private void Start()
        {
            _hazardInitialPosition = _hazard.transform.position;
            _player = GameSystem.GetPlayer();

            InitStages();

            GameSystem.PlayerRespawned += OnPlayerRespawned;
        }

        private void OnDisable()
        {
            GameSystem.PlayerRespawned -= OnPlayerRespawned;
        }

        private void Update()
        {
            if (_inAction)
            {
                MoveHazardObject();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Player _))
            {
                if (!_inAction && CanStart)
                {
                    GameSystem.ShowAreaUntilActionEnd(_hazardCameraFocusPoint, new Action(() =>
                    {
                        StartActivity();
                        GameSystem.PerformAutoSave(_player.transform.position);
                    }), 0.5f);
                }
            }
        }

        private void InitStages()
        {
            foreach (var stage in _stages)
            {
                stage.Init(this);
            }
        }

        public void OnStagePassedByPlayer(EscapeActivityStage stage)
        {
            _pointIndex += 1;
            if (IsLastStage)
            {
                EndActivity();
            }
        }

        private void MoveHazardObject()
        {
            if (_pointIndex == -1)
            {
                return;
            }

            Vector3 pos = _hazard.transform.position;
            pos.x += _stages[_pointIndex].HazardSpeed * Time.deltaTime;
            _hazard.transform.position = pos;
        }

        protected override void StartActivity()
        {
            if (!CanStart)
            {
                return;
            }
            _inAction = true;
            _hazard.TrapEnabled = true;
            _activityEnded = false;
            _pointIndex = 0;
        }

        protected override void EndActivity()
        {
            _pointIndex = -1;
            _hazard.TrapEnabled = false;
            _inAction = false;
            _activityEnded = true;
        }

        protected override void ResetActivity()
        {
            if (_activityEnded)
            {
                return;
            }

            _pointIndex = _inAction ? 0 : -1;
            _hazard.transform.position = _hazardInitialPosition;
        }

        private void OnPlayerRespawned(object sender, EventArgs e)
        {
            ResetActivity();
        }

#if UNITY_EDITOR
        [ContextMenu("Find activity stages")]
        private void FindCheckPoints()
        {
            if (_stages == null)
            {
                _stages = new List<EscapeActivityStage>();
            }
            _stages.Clear();

            Transform stageContainer = transform.GetChild(1);

            for (int i = 0; i < stageContainer.childCount; i++)
            {
                if (stageContainer.GetChild(i).TryGetComponent(out EscapeActivityStage stage))
                {
                    _stages.Add(stage);
                }
            }
        }
#endif
    }
}