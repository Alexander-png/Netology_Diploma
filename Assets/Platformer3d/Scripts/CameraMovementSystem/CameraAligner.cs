using Platformer3d.EditorExtentions;
using System;
using System.Collections;
using UnityEngine;

namespace Platformer3d.CameraMovementSystem
{
	public class CameraAligner : MonoBehaviour
	{
		[SerializeField]
		private Transform _targetPoint;

		[Space, SerializeField, Range(0.5f, 10f)]
		private float _moveSpeed = 5f;

		public event EventHandler ShowAreaExecuted;

        private void OnDisable()
        {
			ShowAreaExecuted = null;
        }

        private void LateUpdate()
        {
			MoveCamera();
		}

		private void MoveCamera()
        {
			if (_targetPoint == null)
            {
				GameLogger.AddMessage($"{gameObject.name}: No target point assigned!", GameLogger.LogType.Fatal);
				return;
            }
			transform.position = Vector3.Lerp(transform.position, _targetPoint.position, _moveSpeed * Time.deltaTime);
		}

		public void ShowAreaUntilActionEnd(Transform position, Action action, float waitTime)
        {
			if (position == null)
            {
				action();
				GameLogger.AddMessage($"{gameObject.name}: Called showing area, but no target position specified.", GameLogger.LogType.Warning);
				return;
            }
			StartCoroutine(ShowAreaCoroutine(position, action, waitTime));
        }

		private IEnumerator ShowAreaCoroutine(Transform position, Action action, float waitTime)
        {
			Transform previousTarget = _targetPoint;
			_targetPoint = position;
			while (!ArrivedToTarget())
            {
				yield return null;
            }
			transform.position = _targetPoint.transform.position;

			action();
			yield return new WaitForSeconds(waitTime);
			_targetPoint = previousTarget;
			ShowAreaExecuted?.Invoke(this, EventArgs.Empty);
		}

		private bool ArrivedToTarget() =>
			Vector3.Distance(transform.position, _targetPoint.transform.position) <= 0.1f;

		private void OnDrawGizmos()
        {
            if (_targetPoint == null)
            {
                return;
            }

			Color color = Color.red;
			color.a = 0.5f;
			Gizmos.color = color;

            Gizmos.DrawSphere(_targetPoint.position, 1f);
        }
    }
}