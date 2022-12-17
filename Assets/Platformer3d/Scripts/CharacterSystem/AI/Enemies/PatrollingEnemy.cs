using Newtonsoft.Json;
using Platformer3d.CharacterSystem.AI.Patroling;
using UnityEngine;

namespace Platformer3d.CharacterSystem.AI.Enemies
{
	public abstract class PatrollingEnemy : Enemy
	{
        [SerializeField, Space(15)]
        protected Transform _patrolArea;
        protected PatrolPoint _currentPoint;

        protected class PatrollingEnemyData : EnemyData
        {
            [JsonIgnore]
            public PatrolPoint CurrentPoint;

            public PatrollingEnemyData(EnemyData enemyData)
            {
                Name = enemyData.Name;
                Side = enemyData.Side;
                RawPosition = enemyData.RawPosition;
                CurrentHealth = enemyData.CurrentHealth;
                AttackingPlayer = enemyData.AttackingPlayer;
                InIdle = enemyData.InIdle;
            }
        }

        protected override void Start()
        {
            FillPatrolPointList();
            base.Start();
        }

        protected virtual void FillPatrolPointList()
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

        public override object GetData()
        {
            PatrollingEnemyData data = new PatrollingEnemyData(base.GetData() as EnemyData)
            {
                CurrentPoint = _currentPoint
            };
            return data;
        }

        public override bool SetData(object data)
        {
            PatrollingEnemyData dataToSet = data as PatrollingEnemyData;
            if (!base.SetData(dataToSet))
            {
                return false;
            }
            _currentPoint = dataToSet.CurrentPoint;
            return true;
        }
    }
}