using GameActors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class SlowOnHit : IOnHitAction
	{
		public float SlowAmount;

		[SuffixLabel("seconds", Overlay = true)]
		public float SlowDuration;

		public void Init(ModularSpell owner)
		{
			owner.AddOnHitAction(this);
		}

		public void OnHit(GameActor actor, Vector3 castDirection, Vector3 movementDirection)
		{
			actor.AddSlow(new SpeedModifier(1 - SlowAmount, SlowDuration));
		}

		public void OnMaxRange(Vector3 position, Vector3 castDirection, Vector3 movementDirection)
		{
		}
	}
}