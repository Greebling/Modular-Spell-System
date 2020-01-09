using System.Linq;
using C;
using GameActors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class ConeCast : IOnCastAction
	{
		[SuffixLabel("units", Overlay = true)] public float Radius;

		[Tooltip("Angle the spell fills out. Set to 360 to cover all directions")]
		[SuffixLabel("degrees", Overlay = true)]
		public float Arc;

		[Tooltip("Hits actors, even if they arent within the angle, but close enough")]
		[SuffixLabel("units", Overlay = true)]
		public float HitRadius = 2;

		[Required] public IOnHitAction[] OnHitActions = new IOnHitAction[0];

		private ModularSpell _owner;
		private Transform _ownerMiddle;

		public void Init(ModularSpell owner)
		{
			_owner = owner;
			
			// Init child actions
			foreach (IOnHitAction onHitAction in OnHitActions)
			{
				onHitAction.Init(owner);
			}

			owner.AddOnCastAction(this);
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			_ownerMiddle = _owner.owner.MidPosition;

			Collider[] hits = Physics.OverlapSphere(_ownerMiddle.position, Radius, Layers.everythingBut(),
				QueryTriggerInteraction.Ignore);

			foreach (Collider collider in hits)
			{
				GameActor currActor = collider.GetComponent<GameActor>();
				if (!currActor) continue;

				if (!_owner.teamsToHit.Contains(currActor.Side)) continue;

				// TODO: Currently broken

				// check if it is within the angle
				float actorAngle = Mathf.Abs(Vector3.SignedAngle(castDirection,
					Vector3.Normalize(currActor.Position - _ownerMiddle.position), Vector3.up));
				if (actorAngle > Arc / 2f) return;

				OnHitActor(currActor, movementDirection);
			}
		}

		/// <summary>
		/// Applys the OnHitEffects to a GameActor
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="movementDirection"></param>
		private void OnHitActor(GameActor actor, Vector3 movementDirection)
		{
			Vector3 castDirection = (actor.MidPosition.position - _ownerMiddle.position).normalized;

			foreach (IOnHitAction onHitAction in OnHitActions)
			{
				onHitAction.OnHit(actor, castDirection, movementDirection);
			}
		}
	}
}