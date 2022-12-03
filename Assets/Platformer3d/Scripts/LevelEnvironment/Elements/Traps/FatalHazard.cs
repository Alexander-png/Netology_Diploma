namespace Platformer3d.Interactables.Elements.Traps
{
	public class FatalHazard : HazardLevelElement
    {
        private void FixedUpdate()
        {
            if (!_trapEnabled || !DamageEnabled)
            {
                return;
            }

            for (int i = 0; i < _touchingCharacters.Count; i++)
            {
                if (_touchingCharacters[i] != null)
                {
                    _touchingCharacters[i].SetDamage(_stats.Damage, transform.up * _stats.PushForce, true);
                }
            }
        }
    }
}