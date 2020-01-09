using System.Linq;
using C;
using GameActors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class LineCast : IOnCastAction
	{
		[SuffixLabel("units", Overlay = true)] public float Range;

		[SuffixLabel("units", Overlay = true)] [Tooltip("The width of the line that is cast")]
		public float LineWidth;
		
		[Required] [Tooltip("The actions that will be executed when a GameActor is hit")] 
		public IOnHitAction[] OnHitActions = new IOnHitAction[0];

		private ModularSpell _owner;
		private Transform _ownerMiddle;

		public void Init(ModularSpell owner)
		{
			_owner = owner;
			
			// Init OnHitActions
			foreach (IOnHitAction onHitAction in OnHitActions)
			{
				onHitAction.Init(owner);
			}

			owner.AddOnCastAction(this);
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			_ownerMiddle = _owner.owner.MidPosition;

			Vector3 boxMiddle = _ownerMiddle.position + castDirection * (Range / 2);
			Vector3 boxHalfExtents = new Vector3(LineWidth / 2, 10, Range / 2);
			
			
			Collider[] hits = Physics.OverlapBox(boxMiddle, boxHalfExtents, Quaternion.LookRotation(castDirection),
				Layers.everythingBut(), QueryTriggerInteraction.Ignore);

			// apply onHits
			foreach (Collider collider in hits)
			{
				GameActor currActor = collider.GetComponent<GameActor>();
				if (!currActor) continue;

				if (!_owner.teamsToHit.Contains(currActor.Side)) continue;

				OnHitActor(currActor, castDirection, movementDirection);
			}
		}

		/// <summary>
		/// Executes all OnHitFunctions
		/// </summary>
		/// <param name="actor"> The actor that is hit</param>
		/// <param name="castDirection"> Direction the spell was cast</param>
		/// <param name="movementDirection"> Movement direction input of the player</param>
		private void OnHitActor(GameActor actor, Vector3 castDirection, Vector3 movementDirection)
		{
			foreach (IOnHitAction onHitAction in OnHitActions)
			{
				onHitAction.OnHit(actor, castDirection, movementDirection);
			}
		}
	}
}