using System.Collections.Generic;
using Enemies;
using GameActors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public abstract class Ability : SerializedMonoBehaviour
	{
		/// <summary>
		/// The gameActor that uses this ability
		/// </summary>
		[HideInEditorMode] [TabGroup("Debug"), PropertyOrder(100)]
		public GameActor owner;
		
		/// <summary>
		/// The Sides the Ability should be able to hit
		/// </summary>
		[HideInEditorMode] [TabGroup("Debug"), PropertyOrder(100)]
		public Team[] teamsToHit;

		/// <summary>
		/// Sets the spells aimed direction
		/// </summary>
		/// <param name="autoAimSnapAngle"></param>
		/// <param name="aimInput"></param>
		/// <param name="movementInput"></param>
		/// <returns> Whether the aiming was successful</returns>
		public abstract bool DoPlayerAimController(float autoAimSnapAngle, Vector3 aimInput, Vector3 movementInput);

		/// <summary>
		/// Uses AI info to aim the spell at one of the targets
		/// </summary>
		/// <param name="targets"></param>
		/// <returns> Whether a valid target was found for the spell </returns>
		public abstract bool DoEnemyAim(List<GameActor> targets);

		/// <summary>
		/// Called when ability should execute. One of the aim methods should have been called by the user of the Ability beforehand
		/// </summary>
		/// <returns> Whether the cast was successful </returns>
		public abstract bool OnCast();
	}
}