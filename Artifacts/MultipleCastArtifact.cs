using Player;
using Spells;
using UnityEngine;

namespace Artifacts
{
	/// <summary>
	/// Makes the spells of the player cast mutliple time instead of just once
	/// </summary>
	public class MultipleCastArtifact : Artifact
	{
		/// <summary>
		/// The amount of times a spell is cast
		/// </summary>
		[Header("Casting")] public int nCasts = 2;
		
		/// <summary>
		/// The time between each cast
		/// </summary>
		public float timeBetweenCasts = 0.3f;

		/// <summary>
		/// Multiplies all damage of a spell that is cast mutliple times
		/// </summary>
		[Header("Damage")] public float damageMultiplier = 0.5f;

		public override string ArtifactName => "Multiply Cast";

		public override void OnPlayerAdd(PlayerController player)
		{
		}

		public override void OnSpellAdd(ModularSpell addedSpell)
		{
			// save onCast actions
			IOnCastAction[] totalOnCastActions = addedSpell.onCastActions.Clone() as IOnCastAction[];

			
			//modify damage of AllOnHitActions
			foreach (IOnHitAction onHitAction in addedSpell.AllOnHitActions)
			{
				if (onHitAction.GetType() == typeof(DamageOnHit))
				{
					DamageOnHit damageAction = onHitAction as DamageOnHit;
					damageAction.damage *= damageMultiplier;
				}
			}


			addedSpell.onCastActions = new IOnCastAction[1];

			// create new multiple cast action
			RepeatCastOnCast castRepeater = new RepeatCastOnCast();
			castRepeater.nRepeats = nCasts;
			castRepeater.timeBetweenCasts = timeBetweenCasts;
			// add old onCast actions to it
			castRepeater.OnCastActions = totalOnCastActions;
			castRepeater.Init(addedSpell);

			
			addedSpell.onCastActions[0] = castRepeater;
		}
	}
}