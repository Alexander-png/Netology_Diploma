using UnityEngine;

namespace Platformer3d.LevelEnvironment.Elements.Common
{
	public class Platform : BaseLevelElement
	{
		[SerializeField]
		private bool _climbable;

		public bool Climbable => _climbable;

        private void OnDrawGizmos()
        {
            Color c = Color.green;
            c.a = 0.6f;
            Gizmos.color = c;

            var collider = GetComponent<BoxCollider>();
            if (collider != null)
            {
                Gizmos.DrawCube(transform.position, collider.size);
            }
        }
    }
}