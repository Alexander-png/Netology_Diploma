using System;

namespace Platformer3d.CharacterSystem.Base
{
	public interface IPlayerInteractable
	{
	    
	}

	public interface IDamagableCharacter
    {
        public float CurrentHealth { get; }
		public void OnGotDamage(float damage, float pushForce);
		public void Heal(float value);

		public event EventHandler OnDied;
    }
}