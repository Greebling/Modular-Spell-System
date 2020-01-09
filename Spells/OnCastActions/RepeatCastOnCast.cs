using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class RepeatCastOnCast : IOnCastAction
	{
		[Required] [Tooltip("The IOnCastAction that shall be repeated")]
		public IOnCastAction[] OnCastActions = new IOnCastAction[0];

		[SuffixLabel("seconds", Overlay = true)]
		public float timeBetweenCasts = 0.25f;

		public int nRepeats = 3;

		private ModularSpell _owner;

		public void Init(ModularSpell owner)
		{
			_owner = owner;

			// Init IOnCastActions
			foreach (IOnCastAction onCastAction in OnCastActions)
			{
				onCastAction.Init(owner);
			}
			
			owner.AddOnCastAction(this);
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			_owner.StartCoroutine(DoCasts(castDirection, movementDirection));
		}

		/// <summary>
		/// Repeates the casting
		/// </summary>
		/// <param name="castDirection"> Direction of the spell cast</param>
		/// <param name="movementDirection"> Movement direction of the owner</param>
		/// <returns></returns>
		private IEnumerator DoCasts(Vector3 castDirection, Vector3 movementDirection)
		{
			for (int i = 0; i < nRepeats; i++)
			{
				foreach (IOnCastAction onCastAction in OnCastActions)
				{
					onCastAction.OnCast(castDirection, movementDirection);
				}

				yield return new WaitForSeconds(timeBetweenCasts);
			}
		}
	}
}