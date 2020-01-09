using System.Collections;
using Player;
using UnityEngine;

namespace Spells
{
	public class DisableGravityOnCast : IOnCastAction
	{
		public float disableDuration;

		private PlayerController _player;
		private ModularSpell _owner;

		public void Init(ModularSpell owner)
		{
			_owner = owner;

			if (_owner.owner)
				_player = _owner.owner.GetComponent<PlayerController>();
			else
				_owner.StartCoroutine(GetPlayer(0.1f));
			
			owner.AddOnCastAction(this);
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			if (_player)
				_player.StartCoroutine(DisableGravity(disableDuration));
		}

		private IEnumerator DisableGravity(float duration)
		{
			_player.usesGravity = false;

			yield return new WaitForSeconds(disableDuration);

			if (_player)
				_player.usesGravity = true;
		}

		private IEnumerator GetPlayer(float delay)
		{
			yield return new WaitForSeconds(delay);
			GetPlayer();
		}

		private void GetPlayer()
		{
			if (_owner.owner)
				_player = _owner.owner.GetComponent<PlayerController>();
			else
			{
				Debug.LogWarning("Spell owner has not been initialized!");
			}
		}
	}
}