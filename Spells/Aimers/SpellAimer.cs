using System.Collections.Generic;
using GameActors;
using UnityEngine;

namespace Spells
{
	/// <summary>
	/// Takes care of the aiming for a spell.
	/// </summary>
	public abstract class SpellAimer
	{
		protected readonly Spell AimedSpell;

		public SpellAimer(Spell aimedSpell)
		{
			Debug.Assert(aimedSpell != null);
			AimedSpell = aimedSpell;
		}
		
		/// <summary>
		/// Rotation going into the aimed direction
		/// </summary>
		public Quaternion AimedRotation { get; protected set; }
		
		/// <summary>
		/// Sets the AimedRotation according to the players input
		/// </summary>
		/// <param name="autoAimSnapAngle"> The angle difference in degrees from the players input
		/// to a possible target where it will still aim at the target</param>
		/// <param name="aimInput"> The players joystick input for aiming </param>
		/// <param name="movementInput"> The player joystick input for movement </param>
		/// <returns></returns>
		public abstract bool DoPlayerAimController(float autoAimSnapAngle, Vector3 aimInput, Vector3 movementInput);

		/// <summary>
		/// Uses AI info to aim the spell at one of the targets
		/// </summary>
		/// <param name="targets"> The list of targets to consider </param>
		/// <returns> Whether a valid target was found for the spell </returns>
		public abstract bool DoEnemyAim(List<GameActor> targets);
	}
}