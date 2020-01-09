using System.Collections.Generic;
using GameActors;
using GameEvents;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Spells
{
	public abstract class Spell : Ability
	{
		[TabGroup("Stats")]
		[SuffixLabel("seconds", Overlay = true)]
		[Tooltip("Time between casts where the spell cannot be used")]
		public float cooldown = 0.5f;

		[TabGroup("Stats")]
		[SuffixLabel("units", Overlay = true)]
		[Tooltip("The range of the spell. Used by the AI to aim its spell and know how far to go towards the player.")]
		public float range = 15;

		/// <summary>
		/// The name of the spell. Used for display in the ingame UI
		/// </summary>
		public virtual string SpellName => "Undefined";


		[TabGroup("Technical")] [Tooltip("The image display in the ingame inventory")]
		public Sprite spellIcon;

		[TabGroup("Technical")]
		[Tooltip("Wether or not a SpellCastEvent is thrown. Usually only player spells should have this checked")]
		public bool throwSpellCastEvent = true;

		/// <summary>
		/// The SpellAimer that gets called to use the players input to aim
		/// <para> Usually overwritten to target a specific SpellAimer Type</para>
		/// </summary>
		public abstract SpellAimer Aimer { get; }

		// used by the cooldown mechanic
		private float _timeSinceLastCast;
		
		/// <summary>
		/// Seconds until the spell can be cast again. Is 0 when the spell is off cooldown. 
		/// </summary>
		public float RemainingCooldown => Mathf.Max(0, cooldown - (Time.time - _timeSinceLastCast));

		/************************** Spellcasting **************************/

		/// <summary>
		/// Used to cast a spell as user. 
		/// </summary>
		/// <returns> Wether the casting was succesful</returns>
		public override bool OnCast()
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (RemainingCooldown == 0)
			{
				if (throwSpellCastEvent)
					GameEventManager.Instance.Raise(new SpellCastEvent());

				if (DoSpellCast())
				{
					_timeSinceLastCast = Time.time;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets called when a spell is successfully activated. Implements the spells functions
		/// </summary>
		/// <returns> Whether the cast was successful</returns>
		protected abstract bool DoSpellCast();

		/************************** Aiming **************************/

		public override bool DoPlayerAimController(float autoAimSnapAngle, Vector3 aimInput, Vector3 movementInput)
		{
			return Aimer.DoPlayerAimController(autoAimSnapAngle, aimInput, movementInput);
		}

		/// <summary>
		/// Uses AI info to aim the spell at one of the targets
		/// </summary>
		/// <param name="targets"> The list of targets to consider </param>
		/// <returns> Whether a valid target was found for the spell </returns>
		public override bool DoEnemyAim(List<GameActor> targets)
		{
			return Aimer.DoEnemyAim(targets);
		}

		/************************** Inventory **************************/

		/// <summary>
		/// Called when spell is in the players inventory
		/// </summary>
		public virtual void OnInventoryUpdate()
		{
		}

		/// <summary>
		/// Called when spell removed from the players inventory
		/// </summary>
		public virtual void OnInventoryRemove()
		{
		}

		/// <summary>
		/// Called when spell is added to players inventory
		/// </summary>
		public virtual void OnInventoryAdd(GameActor nextOwner)
		{
			owner = nextOwner;

			_timeSinceLastCast = -cooldown;
		}
	}
}