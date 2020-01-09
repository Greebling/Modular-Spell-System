using GameActors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class HealOnCast : IOnCastAction
	{
		[HorizontalGroup("Amount")] public float HealAmount;

		[HorizontalGroup("Amount")] [ToggleLeft]
		public bool IsPercentMaxHp;


		private ModularSpell _owner;

		public void Init(ModularSpell owner)
		{
			_owner = owner;
			
			owner.AddOnCastAction(this);
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			HealActorInstant(_owner.owner);
		}

		private void HealActorInstant(GameActor actor)
		{
			float healAmount = IsPercentMaxHp ? actor.MaxHealth * HealAmount : HealAmount;

			actor.Health += healAmount;
		}
		
		public void OnMaxRange(Vector3 position, Vector3 forwardVector)
		{
		}
	}
}