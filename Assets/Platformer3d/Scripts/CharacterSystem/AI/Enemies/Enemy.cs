using Platformer3d.CharacterSystem.AI.Patroling;
using Platformer3d.CharacterSystem.Base;
using Platformer3d.CharacterSystem.Movement;
using Platformer3d.GameCore;
using Platformer3d.PlayerSystem;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Platformer3d.CharacterSystem.AI.Enemies
{
	public class Enemy : MoveableCharacter
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

        private EnemyMovement _movement;
        private Player _player;
        private PatrolPoint _currentPoint;

        private bool _inIdle;

        private bool _attackingPlayer = false;

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
                _movement.SetVelocity(Vector3.zero);
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

        }

        private void AttackPlayer()
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
            Color c = Color.red;
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