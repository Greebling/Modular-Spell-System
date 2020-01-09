using System.Collections.Generic;
using GameActors;
using UnityEngine;

namespace Spells
{
	public class AuraAimer : SpellAimer
	{
		public AuraAimer(Spell aimedSpell) : base(aimedSpell)
		{
		}

		public override bool DoPlayerAimController(float autoAimSnapAngle, Vector3 aimInput, Vector3 movementInput)
		{
			return true;
		}

		public override bool DoEnemyAim(List<GameActor> targets)
		{
			float minDistance = float.MaxValue;

			foreach (GameActor target in targets)
			{
				float currDist = Vector3.Distance(target.MidPosition.position, AimedSpell.transform.position);
				if (currDist < minDistance)
				{
					minDistance = currDist;
				}
			}

			return minDistance <= AimedSpell.range;
		}
	}
}