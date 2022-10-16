using Platformer3d.CharacterSystem.Base;
using Platformer3d.LevelEnvironment.Elements.Base;
using Platformer3d.Scriptable;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer3d.LevelEnvironment.Elements
{
	public class DangerLevelElement : BaseLevelElement
	{
		[SerializeField]
		private DamageStats _stats;

        private List<IDamagableCharacter> _touchingCharacters = new List<IDamagableCharacter>();

        private void OnEnable()
        {
            GameSystem.OnPlayerDied += OnPlayerDied;
        }

        private void OnDisable()
        {
            GameSystem.OnPlayerDied -= OnPlayerDied;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamagableCharacter character))
            {
                _touchingCharacters.Add(character);
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _touchingCharacters.Count; i++)
            {
                if (_touchingCharacters[i] != null)
                {
                    _touchingCharacters[i].OnGotDamage(_stats.Damage, _stats.PushForce);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamagableCharacter character))
            {
                _touchingCharacters.Remove(character);
            }
        }

        private void OnPlayerDied(object sender, System.EventArgs e)
        {
            _touchingCharacters.Clear();
        }
    }
}