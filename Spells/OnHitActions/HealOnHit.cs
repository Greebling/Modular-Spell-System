using GameActors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class HealOnHit : IOnHitAction
	{
		[HorizontalGroup("Amount")] public float HealAmount;

		[HorizontalGroup("Amount")] [ToggleLeft]
		public bool IsPercentMaxHp;

		public bool HealCaster = true;
		public bool HealHitActor;


		private ModularSpell _owner;

		public void Init(ModularSpell owner)
		{
			_owner = owner;
			owner.AddOnHitAction(this);
		}

		public void OnHit(GameActor actor, Vector3 castDirection, Vector3 movementDirection)
		{
			if (HealCaster)
			{
				HealActorInstant(_owner.owner);
			}

			if (HealHitActor)
			{
				HealActorInstant(actor);
			}
		}

		public void OnMaxRange(Vector3 position, Vector3 castDirection, Vector3 movementDirection)
		{
		}

		private void HealActorInstant(GameActor actor)
		{
			float healAmount = IsPercentMaxHp ? actor.MaxHealth * HealAmount : HealAmount;

			actor.Health += healAmount;
		}
	}
}