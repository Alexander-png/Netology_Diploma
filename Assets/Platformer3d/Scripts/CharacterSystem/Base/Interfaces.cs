using Platformer3d.SkillSystem;
using System;

namespace Platformer3d.CharacterSystem.Base
{
	public interface IPlayerInteractable
	{
	    
	}

	public interface IDamagableCharacter
    {
        public float CurrentHealth { get; }
		public void SetDamage(float damage, float pushForce);
		public void Heal(float value);

		public event EventHandler Died;
    }

	public interface ISkillObservable
    {
		public SkillObserver SkillObserver { get; }
    }
}