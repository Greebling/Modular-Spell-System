using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class DelayCast : IOnCastAction
	{
		[SuffixLabel("seconds", Overlay = true)]
		public float castDelay;

		[Tooltip("The IOnCastActions that shall use the rotated Inputs")]
		public IOnCastAction[] castActions = new IOnCastAction[0];

		private ModularSpell _owner;

		public void Init(ModularSpell owner)
		{
			_owner = owner;

			// Init child actions
			foreach (IOnCastAction onCastAction in castActions)
			{
				onCastAction.Init(owner);
			}
			
			owner.AddOnCastAction(this);
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			_owner.StartCoroutine(DelayedCast(castDelay,
				new Vector3(castDirection.x, castDirection.y, castDirection.z),
				new Vector3(movementDirection.x, movementDirection.y, movementDirection.z)));
		}

		private IEnumerator DelayedCast(float delay, Vector3 castDirection, Vector3 movementDirection)
		{
			yield return new WaitForSeconds(delay);

			// cast actions
			foreach (IOnCastAction onCastAction in castActions)
			{
				onCastAction.OnCast(castDirection, movementDirection);
			}
		}
	}
}