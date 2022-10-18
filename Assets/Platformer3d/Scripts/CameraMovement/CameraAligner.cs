using Platformer3d.EditorExtentions;
using UnityEngine;

namespace Platformer3d.CameraMovement
{
	public class CameraAligner : MonoBehaviour
	{
		public enum StrictModeTypes : byte
        {
			NonStrict = 0,
			Strict = 1,
        }

		[SerializeField]
		private Transform _targetPoint;
		[Space, SerializeField, Range(0.5f, 10f)]
		private float _moveSpeed = 5f;
		//[SerializeField, Range(0.1f, 3f)]
		//private float _acceleration = 0.5f;
		//[SerializeField, Range(0.5f, 5f)]
		//private float _stayRange = 2;
        [SerializeField]
		private StrictModeTypes _strictMode = StrictModeTypes.NonStrict;

		private bool _shouldMove;

		private void LateUpdate()
        {
			MoveCamera();
		}

		private void MoveCamera()
        {
			if (_targetPoint == null)
            {
				GameLogger.AddMessage($"{nameof(CameraAligner)}: No target point assigned!", GameLogger.LogType.Fatal);
				return;
            }
			//if (CheckShouldMove())
			//{

			//}
			//transform.position = Vector3.Lerp(transform.position, _targetPoint.position, _moveSpeed * TimeSystem.Instance.ScaledGameDeltaTime);

			transform.position = Vector3.Lerp(transform.position, _targetPoint.position, _moveSpeed * Time.deltaTime);
		}

        //private bool CheckShouldMove()
        //{
        //    if (_strictMode == StrictModeTypes.NonStrict)
        //    {
        //        float distance = Vector2.Distance(transform.position, _targetPoint.position);
        //        if (distance > _stayRange && !_shouldMove)
        //        {
        //            _shouldMove = true;
        //        }
        //        else if (distance < 0.1f)
        //        {
        //            _shouldMove = false;
        //        }
        //    }
        //    else if (_strictMode == StrictModeTypes.Strict)
        //    {
        //        _shouldMove = true;
        //    }
        //    return _shouldMove;
        //}

        private void OnDrawGizmos()
        {
            if (_targetPoint == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_targetPoint.position, 1f);
        }

        public void SetStrictMode(StrictModeTypes mode) => _strictMode = mode;
    }
}