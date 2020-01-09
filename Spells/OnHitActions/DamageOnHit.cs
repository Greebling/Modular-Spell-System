using GameActors;
using UnityEngine;

namespace Spells
{
	public class DamageOnHit : IOnHitAction
	{
		[Tooltip("The amount of damage that is being dealt to the hit GameActor")]
		public float damage;

		public void Init(ModularSpell owner)
		{
			owner.AddOnHitAction(this);
		}

		public void OnHit(GameActor actor, Vector3 castDirection, Vector3 movementDirection)
		{
			actor.Health -= damage;
		}

		public void OnMaxRange(Vector3 position, Vector3 castDirection, Vector3 movementDirection)
		{
		}
	}
}