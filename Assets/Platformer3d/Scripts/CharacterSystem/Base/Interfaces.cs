using Platformer3d.SkillSystem;
using System;
using UnityEngine;

namespace Platformer3d.CharacterSystem.Base
{
	public interface IPlayerInteractable
	{
	    
	}

	public interface IDamagableCharacter
    {
        public float CurrentHealth { get; }
		public void SetDamage(float damage, Vector3 pushVector, bool forced = false);
		public void Heal(float value);

		public event EventHandler Died;
    }

	public interface ISkillObservable
    {
		public SkillObserver SkillObserver { get; }
    }
}