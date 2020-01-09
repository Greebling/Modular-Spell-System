using System.Collections.Generic;
using C;
using Enemies;
using GameActors;
using UnityEngine;

namespace Spells
{
	/// <summary>
	/// Aims at the position under the players mouse cursor, copying the y coordinate of the spell,
	/// via a <c>Vector3</c> in form of a forward coordinate going from the spells position to the mouse cursor position
	/// </summary>
	public class SkillshotAimer : SpellAimer
	{
		public SkillshotAimer(Spell aimedSpell, float projectileSpeed, float leadAmount) : base(aimedSpell)
		{
			_projectileSpeed = projectileSpeed;
			_leadAmount = leadAmount;
		}

		private readonly float _projectileSpeed, _leadAmount;

		/// <summary>
		/// Direction where the spell is aimed at
		/// </summary>
		public Vector3 AimedDirection { get; protected set; }

		public Vector3 MovementDirection { get; protected set; }

		protected virtual bool CheckSightline => true;
		protected virtual bool CheckRange => true;

		/// <summary>
		/// Automatically aims at the enemy that fits best to the direction the player is currently moving to
		/// </summary>
		/// <returns></returns>
		public override bool DoPlayerAimController(float autoAimSnapAngle, Vector3 aimInput, Vector3 movementInput)
		{
			MovementDirection = movementInput;
			
			float bestScore = float.MaxValue;

			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (autoAimSnapAngle == 0)
			{
				//Debug.Log("Aborting auto aim");

				AimedDirection = Mathf.Abs(aimInput.sqrMagnitude) >= 0.02f
					? aimInput
					: AimedSpell.transform.forward;

				AimedDirection = Vector3.Normalize(AimedDirection);
				AimedRotation = Quaternion.LookRotation(AimedDirection);
				return true;
			}

			// check if the player inputted directions at all
			if (aimInput.sqrMagnitude <= 0.01f)
			{
				aimInput = AimedSpell.owner.transform.forward;
			}

			float squaredRange = AimedSpell.range * AimedSpell.range;

			foreach (Enemy enemy in Enemy.Enemies)
			{
				Debug.Assert(enemy != null);

				// distance check
				float distanceToEnemy = (AimedSpell.transform.position - enemy.MidPosition.position).sqrMagnitude;
				if (distanceToEnemy > squaredRange)
				{
					//Debug.Log(enemy.name + " is too far away");
					continue;
				}


				float angleDifference = Mathf.Abs(Vector3.SignedAngle(aimInput,
					enemy.MidPosition.position - AimedSpell.transform.position, Vector3.up));
				//Debug.Log("Angle difference: " +angleDifference);

				if (angleDifference <= autoAimSnapAngle && angleDifference <= bestScore)
				{
					bestScore = angleDifference;
					AimAtGameActor(enemy);

					//Debug.Log("Targeting " + enemy.name);
				}
			}


			// if no target was snapped, just throw the spell into the aimed direction
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (bestScore == float.MaxValue)
			{
				AimedDirection = aimInput;
				AimedDirection = Vector3.Normalize(AimedDirection);
				AimedRotation = Quaternion.LookRotation(AimedDirection);
			}


			return true;
		}

		public override bool DoEnemyAim(List<GameActor> targets)
		{
			Debug.Assert(targets != null);
			Debug.Assert(targets.Count > 0);

			float minDistance = float.MaxValue;
			GameActor bestTarget = null;

			foreach (GameActor target in targets)
			{
				float currDist = Vector3.Distance(target.MidPosition.position, AimedSpell.transform.position);

				if (CheckRange && AimedSpell.range < currDist)
					continue;

				if (currDist < minDistance)
				{
					// check line of sight
					if (CheckSightline && Physics.Linecast(AimedSpell.transform.position, target.MidPosition.position,
						    out RaycastHit lineOfSightHit,
						    ~((1 << Layers.ENEMIES) | (1 << Layers.ENEMY_SPELLS) | (1 << Layers.PLAYER_ACESSOIRS))))
					{
						// hit player?
						if (!lineOfSightHit.collider.CompareTag(Tags.PLAYER))
						{
							//Debug.Log("sees " + lineOfSightHit.collider.name);
							continue;
						}
					}

					minDistance = currDist;
					bestTarget = target;
				}
			}

			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (minDistance == float.MaxValue)
			{
				return false; // found nothing
			}

			AimAtGameActor(bestTarget);
			return true;
		}

		/// <summary>
		/// Leads the aim
		/// </summary>
		/// <param name="target"></param>
		private void AimAtGameActor(GameActor target)
		{
			if (target.IsMoving)
			{
				float flightTime = Vector3.Distance(AimedSpell.transform.position, target.MidPosition.position) /
				                   _projectileSpeed;
				// calculate the distance the target moves in that time
				float distanceMoved = flightTime * target.Speed;

				AimAtPosition(target.MidPosition.position + _leadAmount * distanceMoved * target.transform.forward);
			}
			else
			{
				AimAtPosition(target.MidPosition.position);
			}
		}

		private void AimAtPosition(Vector3 target)
		{
			AimedDirection = target - AimedSpell.transform.position;
			AimedDirection = new Vector3(AimedDirection.x, 0, AimedDirection.z);
			AimedDirection = Vector3.Normalize(AimedDirection);

			AimedRotation = Quaternion.LookRotation(AimedDirection);
		}
	}
}