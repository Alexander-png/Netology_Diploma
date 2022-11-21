using Platformer3d.CharacterSystem.AI.Patroling;
using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.Movement;
using Platformer3d.GameCore;
using Platformer3d.PlayerSystem;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Platformer3d.CharacterSystem.AI.Enemies
{
	public class Enemy : MoveableCharacter, IDamagableCharacter
    {
		[Inject]
		private GameSystem _gameSystem;

        [SerializeField]
        private BoxCollider _agressionTrigger;
        [SerializeField]
        private Transform _visual;

        [SerializeField, Space(15)]
        private Transform _patrolArea;
        [SerializeField]
        private float _idleTime;
        [SerializeField]
        private float _attackRadius;

        private EnemyMovement _movement;
        private Player _player;
        private PatrolPoint _currentPoint;

        private bool _inIdle;

        private bool _attackingPlayer = false;

        public event EventHandler Died;

        public float CurrentHealth => throw new NotImplementedException();

        protected override void Start()
        {
            _player = _gameSystem.GetPlayer();
            _movement = MovementController as EnemyMovement;
            MovementController.MovementEnabled = true;
            FillPatrolPointList();
        }

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
            if (other.TryGetComponent(out Player player))
            {
                _attackingPlayer = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _attackingPlayer = false;
            }
        }

        protected override void Update()
        {
            UpdateBehaviour();
        }

        private void UpdateBehaviour()
        {
            if (!_attackingPlayer)
            {
                PatrolArea();
            }
            else
            {
                PursuitPlayer();
                AttackPlayer();
            }
        }

        private void PatrolArea()
        {
            if (_inIdle)
            {
                _movement.MoveInput = 0f;
                _movement.Velocity = Vector3.zero;
                return;
            }

            var pointPos = _currentPoint.Position;

            if (pointPos.x > transform.position.x)
            {
                _visual.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
                _movement.MoveInput = 1f;
            }
            else
            {
                _visual.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                _movement.MoveInput = -1f;
            }

            if (Vector3.SqrMagnitude(transform.position - _currentPoint.Position) <= _currentPoint.ArriveRadius)
            {
                _currentPoint = _currentPoint.NextPoints[0];
                StartCoroutine(IdleCoroutine(_idleTime));
            }
        }

        private void PursuitPlayer()
        {
            // TODO: pursuit behaviour
            // TODO: kill enemy ability
            // TODO: reset on player died
        }

        private void AttackPlayer()
        {

        }

        public void SetDamage(float damage, Vector3 pushVector)
        {
            
        }

        public void Heal(float value)
        {
            
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