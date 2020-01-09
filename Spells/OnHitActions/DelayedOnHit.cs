using System.Collections;
using GameActors;
using UnityEngine;

namespace Spells
{
	public class DelayedOnHit : IOnHitAction
	{
		[SuffixLabel("seconds", Overlay = true)]
		public float delayAmount;
		
		[Tooltip("The IOnHitActions whose cast will be delayed")]
		public IOnHitAction[] OnHitActions = new IOnHitAction[0];

		private ModularSpell _owner;

		public void Init(ModularSpell owner)
		{
			_owner = owner;

			// Init the OnHitActions
			foreach (IOnHitAction onHitAction in OnHitActions)
			{
				onHitAction.Init(owner);
			}
		}

		public void OnHit(GameActor actor, Vector3 castDirection, Vector3 movementDirection)
		{
			_owner.StartCoroutine(DelayOnHitEffects(delayAmount, actor, castDirection, movementDirection));
		}

		public void OnMaxRange(Vector3 position, Vector3 castDirection, Vector3 movementDirection)
		{
			_owner.StartCoroutine(DelayOnMaxRangeEffects(delayAmount, position, castDirection, movementDirection));
		}

		/// <summary>
		/// Executes the OnHitActions after a certain delay.
		/// </summary>
		/// <param name="delay"> Delay in seconds</param>
		/// <param name="actor"> GameActor that was hit</param>
		/// <param name="castDirection"> Direction the spell was cast</param>
		/// <param name="movementDirection"> Movement direction the player inputted</param>
		/// <returns></returns>
		private IEnumerator DelayOnHitEffects(float delay, GameActor actor, Vector3 castDirection,
			Vector3 movementDirection)
		{
			yield return new WaitForSeconds(delay);

			foreach (IOnHitAction onHitAction in OnHitActions)
			{
				onHitAction.OnHit(actor, castDirection, movementDirection);
			}
		}

		/// <summary>
		/// Executes the OnHitActions OnMaxRange functions after a certain delay.
		/// </summary>
		/// <param name="delay"> Delay in seconds</param>
		/// <param name="position"> Position where the spell hit it's max range</param>
		/// <param name="castDirection"> Direction the spell was cast</param>
		/// <param name="movementDirection"> Movement direction the player inputted</param>
		/// <returns></returns>
		private IEnumerator DelayOnMaxRangeEffects(float delay, Vector3 position, Vector3 castDirection,
			Vector3 movementDirection)
		{
			yield return new WaitForSeconds(delay);

			foreach (IOnHitAction onHitAction in OnHitActions)
			{
				onHitAction.OnMaxRange(position, castDirection, movementDirection);
			}
		}
	}
}