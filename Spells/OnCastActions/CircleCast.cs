using System.Linq;
using C;
using GameActors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class CircleCast : IOnCastAction
	{
		[SuffixLabel("units", Overlay = true)] [Tooltip("The radius of the circle that is cast")]
		public float Radius = 10;

		[Tooltip("Whether or not the circle has a minimum range. Creates a donut like shape if set to true.")]
		public bool hasInnerRadius;

		[ShowIf("hasInnerRadius")] [Tooltip("The minmum range of the circle")]
		public float innerRadius = 1;

		[Required] public IOnHitAction[] OnHitActions = new IOnHitAction[0];

		private ModularSpell _owner;
		private Transform _ownerMiddle;

		/// <summary>
		/// Is equal to the innerRadius squared. Used for more efficient distance checking
		/// </summary>
		private float _squaredInnerRadius;

		public void Init(ModularSpell owner)
		{
			_owner = owner;
			
			// Init child actions
			foreach (IOnHitAction onHitAction in OnHitActions)
			{
				onHitAction.Init(owner);
			}

			_squaredInnerRadius = innerRadius * innerRadius;

			owner.AddOnCastAction(this);
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			_ownerMiddle = _owner.owner.MidPosition;

			// ReSharper disable once Unity.PreferNonAllocApi
			Collider[] hits = Physics.OverlapSphere(_ownerMiddle.position, Radius, Layers.everythingBut(),
				QueryTriggerInteraction.Ignore);


			foreach (Collider collider in hits)
			{
				// check inner radius
				if (hasInnerRadius && _squaredInnerRadius <=
				    Vector3.SqrMagnitude(collider.transform.position - _ownerMiddle.position))
				{
					continue;
				}

				GameActor currActor = collider.GetComponent<GameActor>();
				if (!currActor) continue;

				if (!_owner.teamsToHit.Contains(currActor.Side)) continue;


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