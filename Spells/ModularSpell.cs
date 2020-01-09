using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Spells
{
	public class ModularSpell : Spell
	{
		[TabGroup("Technical")] public string spellname;


		/// <summary>
		/// The IOnCastActions that will be executed when the spell is cast
		/// </summary>
		public IOnCastAction[] onCastActions = new IOnCastAction[0];

		// override the SpellAimer of the spell class to use a SkillshotAimer
		public override SpellAimer Aimer => _skillshotAimer;
		private SkillshotAimer _skillshotAimer;

		public override string SpellName => spellname;

		/// <summary>
		/// A list of all AllOnHitActions that belong to this ModularSpell
		/// </summary>
		[NonSerialized] public List<IOnHitAction> AllOnHitActions = new List<IOnHitAction>();

		/// <summary>
		/// A list of all IOnCastActions that belong to this ModularSpell
		/// </summary>
		[NonSerialized] public List<IOnCastAction> AllOnCastActions = new List<IOnCastAction>();

		private void Start()
		{
			_skillshotAimer = new SkillshotAimer(this, 1, 0);
			
			foreach (IOnCastAction onCastAction in onCastActions)
			{
				onCastAction.Init(this);
			}
		}

		/// <summary>
		/// Gets called when a spell is successfully activated. Implements the spells functions
		/// </summary>
		/// <returns> Whether the cast was successful</returns>
		protected override bool DoSpellCast()
		{
			foreach (IOnCastAction onCastAction in onCastActions)
			{
				onCastAction.OnCast(_skillshotAimer.AimedDirection, _skillshotAimer.MovementDirection);
			}

			return true;
		}

		public void AddOnHitAction(IOnHitAction action)
		{
			AllOnHitActions.Add(action);
		}

		public void AddOnCastAction(IOnCastAction action)
		{
			AllOnCastActions.Add(action);
		}

		private void OnDestroy()
		{
			StopAllCoroutines();
		}
	}
}