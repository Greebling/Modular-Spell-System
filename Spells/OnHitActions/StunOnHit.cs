using GameActors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class StunOnHit : IOnHitAction
	{
		[SuffixLabel("seconds", Overlay = true)]
		public float StunDuration;

		public void Init(ModularSpell owner)
		{
			owner.AddOnHitAction(this);
		}

		public void OnHit(GameActor actor, Vector3 castDirection, Vector3 movementDirection)
		{
			actor.AddStun(new StunModifier(StunDuration));
		}

		public void OnMaxRange(Vector3 position, Vector3 castDirection, Vector3 movementDirection)
		{
		}
	}
}