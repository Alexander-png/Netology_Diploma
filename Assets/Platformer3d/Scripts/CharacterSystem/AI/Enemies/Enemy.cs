using Platformer3d.CharacterSystem.AI.Patroling;
using Platformer3d.CharacterSystem.Base;
using Platformer3d.GameCore;
using Platformer3d.PlayerSystem;
using Platformer3d.Scriptable.Characters;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Platformer3d.CharacterSystem.AI.Enemies
{
	public class Enemy : MoveableCharacter, IDamagableCharacter, ISaveable
    {
		[Inject]
		private GameSystem _gameSystem;

        [SerializeField]
        private BoxCollider _agressionTrigger;

        // TODO: better to move these fields to scriptable object
        [SerializeField, Space(15)]
        private Transform _patrolArea;
        [SerializeField]
        private float _idleTime;
        [SerializeField]
        private float _attackRadius;
        [SerializeField]
        private float _playerHeightDiffToJump;
        [SerializeField]
        private float _closeToPlayerDistance;
        
        private float _currentHealth;
        private float _maxHealth;

        private Player _player;
        private PatrolPoint _currentPoint;

        private bool _behaviourEnabled;
        private bool _inIdle;
        private bool _attackingPlayer = false;

        public event EventHandler Died;

        protected class EnemyData : CharacterData
        {
            public bool AttackingPlayer;
            public bool InIdle;
            public PatrolPoint CurrentPoint;
        }

        public float CurrentHealth => _currentHealth;

        protected override void Start()
        {
            _gameSystem.RegisterSaveableObject(this);

            _player = _gameSystem.GetPlayer();
            MovementController.MovementEnabled = true;
            FillPatrolPointList();
            SetBehaviourEnabled(true);
        }

        public bool SetBehaviourEnabled(bool value) => 
            _behaviourEnabled = value;

        private void FillPatrolPointList()
        {
            for (int i = 0; i < _patrolArea.childCount; i++)
            {
                if (_patrolArea.GetChild(i).TryGetComponent(out PatrolPoint point))
                {
                    _currentPoint = point;
                    break;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out MoveableCharacter _))
            {
                Vector3 newVelocity = (-MovementController.Velocity + transform.up).normalized;
                newVelocity *= MovementController.MaxJumpForce;
                MovementController.Velocity = newVelocity;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _inIdle = false;
                _attackingPlayer = true;
            }
        }

        // TODO: fix OnTriggerExit call when character may being attacked with weapon

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _attackingPlayer = false;
            }
        }

        protected override void SetDefaultParameters(DefaultCharacterStats stats)
        {
            base.SetDefaultParameters(stats);
            _maxHealth = stats.MaxHealth;
            _currentHealth = _maxHealth;
        }

        protected override void Update()
        {
            base.Update();
            UpdateBehaviour();
        }

        private void UpdateBehaviour()
        {
            if (!_behaviourEnabled)
            {
                MovementController.MoveInput = 0f;
                _attackingPlayer = false;
                return;
            }

            if (!_attackingPlayer)
            {
                PatrolArea();
            }
            else
            {
                PursuitPlayer();
            }
        }

        private void PatrolArea()
        {
            if (_inIdle)
            {
                MovementController.MoveInput = 0f;
                MovementController.Velocity = Vector3.zero;
                return;
            }

            var pointPos = _currentPoint.Position;

            MovementController.MoveInput = pointPos.x > transform.position.x ? 1f : -1f;

            if (Vector3.SqrMagnitude(transform.position - _currentPoint.Position) <= _currentPoint.ArriveRadius)
            {
                _currentPoint = _currentPoint.NextPoints[0];
                StartCoroutine(IdleCoroutine(_idleTime));
            }
        }

        private void PursuitPlayer()
        {
            Vector3 playerPosition = _player.transform.position;
            Vector3 selfPosition = transform.position;
            if (playerPosition.x > selfPosition.x)
            {
                MovementController.MoveInput = 1f;
            }
            else if (playerPosition.x < selfPosition.x)
            {
                MovementController.MoveInput = -1f;
            }

            bool closeToPlayer = Mathf.Abs(playerPosition.x - selfPosition.x) <= _closeToPlayerDistance;

            if (closeToPlayer)
            {
                bool needToJump = playerPosition.y - selfPosition.y >= _playerHeightDiffToJump;
                if (needToJump)
                {
                    MovementController.JumpInput = 1f;
                }
                else
                {
                    MovementController.JumpInput = 0f;
                }
            }
            else
            {
                MovementController.JumpInput = 0f;
            }
        }

        public void SetDamage(float damage, Vector3 pushVector)
        {
            MovementController.Velocity = pushVector;
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
            if (_currentHealth < 0.01f)
            {
                SetBehaviourEnabled(false);
                EditorExtentions.GameLogger.AddMessage($"Enemy with name {gameObject.name} was. Killed. You can implement spawn system");
                gameObject.SetActive(false);
            }
        }

        public void Heal(float value) =>
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _maxHealth);

        public object GetData() => new EnemyData()
        {
            Name = gameObject.name,
            Side = Side,
            Position = transform.position,
            CurrentHealth = CurrentHealth,
            AttackingPlayer = _attackingPlayer,
            InIdle = _inIdle,
            CurrentPoint = _currentPoint
        };

        public void SetData(object data)
        {
            EnemyData dataToSet = data as EnemyData;
            if (!ValidateData(dataToSet))
            {
                return;
            }

            Side = dataToSet.Side;
            transform.position = dataToSet.Position;
            _currentHealth = dataToSet.CurrentHealth;
            _attackingPlayer = dataToSet.AttackingPlayer;
            _inIdle = dataToSet.InIdle;
            _currentPoint = dataToSet.CurrentPoint;
        }

        private IEnumerator IdleCoroutine(float idleTime)
        {
            _inIdle = true;
            yield return new WaitForSeconds(idleTime);
            _inIdle = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color c = Color.gray;
            c.a = 0.3f;
            Gizmos.color = c;

            if (_agressionTrigger != null)
            {
                Vector3 rangeVisualSize = _agressionTrigger.size;
                Gizmos.DrawCube(transform.position + _agressionTrigger.center, rangeVisualSize);
            }
        }
#endif
    }
}