using GameActors;
using UnityEngine;

namespace Spells
{
	/// <summary>
	/// Executes OnCastActions when a GameActor is hit
	/// </summary>
	public class CastActionOnHit : IOnHitAction
	{
		[Tooltip("The actions that will be executed when a GameActor is hit")]
		public IOnCastAction[] onCastActions;
		
		[Tooltip("Wether or not the onCastActions copy the direction of the original cast or go into the opposite direction")]
		public bool sameCastDirection;
		
		[Tooltip("Wether or not the actions shall be executed when the spell hit nothing but reached it's maximum range")]
		public bool castOnMaxRange;

		public void Init(ModularSpell owner)
		{
			// Init onCastActions
			foreach (IOnCastAction onCastAction in onCastActions)
			{
				onCastAction.Init(owner);
			}

			owner.AddOnHitAction(this);
		}

		public void OnHit(GameActor actor, Vector3 castDirection, Vector3 movementDirection)
		{
			DoCast(castDirection, movementDirection);
		}

		public void OnMaxRange(Vector3 position, Vector3 castDirection, Vector3 movementDirection)
		{
			if (castOnMaxRange)
			{
				DoCast(castDirection, movementDirection);
			}
		}

		/// <summary>
		/// Executes the onCastActions
		/// </summary>
		/// <param name="forwardVector"> The direction the spell was cast</param>
		/// <param name="movementDirection"> The movement input of the user</param>
		private void DoCast(Vector3 forwardVector, Vector3 movementDirection)
		{
			if (!sameCastDirection) forwardVector *= -1;

			foreach (IOnCastAction onCastAction in onCastActions)
			{
				onCastAction.OnCast(forwardVector, movementDirection);
			}
		}
	}
}