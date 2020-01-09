using System.Collections.Generic;
using Enemies;
using GameActors;
using UnityEngine;

namespace Spells
{
	/// <summary>
	/// Aims directly at the position under the mouse cursor.
	/// <para>Note that this gives a definite position to be aimed at as opposed to a direction in form of a forward coordinate in <c>SkillshotSpell</c></para>
	/// </summary>
	public class MousePositionAimer : SpellAimer
	{
		public MousePositionAimer(Spell aimedSpell) : base(aimedSpell)
		{
		}

		/// <summary>
		/// Position where the spell is aimed at
		/// </summary>
		public Vector3 AimedPosition { get; private set; }

		/// <summary>
		/// Automatically aims at the enemy that fits best to the direction the player is currently moving to
		/// </summary>
		/// <returns></returns>
		public override bool DoPlayerAimController(float autoAimSnapAngle, Vector3 aimInput, Vector3 movementInput)
		{
			float bestScore = float.MaxValue;

			// check if the player inputted directions at all
			if (aimInput.sqrMagnitude <= 0.02f)
			{
				aimInput = AimedSpell.owner.transform.forward;
			}

			foreach (Enemy enemy in Enemy.Enemies)
			{
				Debug.Assert(enemy != null);

				// distance check
				float distanceToEnemy = Vector3.Distance(AimedSpell.transform.position, enemy.MidPosition.position);
				if (distanceToEnemy > AimedSpell.range) continue;

				float angleDifference = Mathf.Abs(Vector3.SignedAngle(aimInput,
					enemy.MidPosition.position - AimedSpell.transform.position, Vector3.up));

				// evaluate scoring

				if (angleDifference <= autoAimSnapAngle && angleDifference <= bestScore)
				{
					bestScore = angleDifference;
					AimedPosition = enemy.MidPosition.position;
				}
			}

			// check if bestScore was set
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (bestScore != float.MaxValue)
			{
				// Do Rotation
				AimedRotation = Quaternion.LookRotation(AimedPosition - AimedSpell.transform.position);
				return true;
			}

			return false;
		}

		public override bool DoEnemyAim(List<GameActor> targets)
		{
			Debug.Assert(targets != null);
			Debug.Assert(targets.Count > 0);

			float minDistance = float.MaxValue;
			Vector3 bestTarget = Vector3.zero;

			foreach (GameActor target in targets)
			{
				float currDist = Vector3.Distance(target.MidPosition.position, AimedSpell.transform.position);
				if (currDist < minDistance)
				{
					minDistance = currDist;
					bestTarget = target.MidPosition.position;
				}
			}

			if (minDistance > AimedSpell.range) return false; // found nothing


			AimedPosition = bestTarget;
			AimedRotation = Quaternion.LookRotation(bestTarget - AimedSpell.transform.position);

			return true;
		}
	}
}