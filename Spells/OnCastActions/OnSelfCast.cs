using UnityEngine;

namespace Spells
{
	/// <summary>
	/// Applys the OnHitActions to the caster
	/// </summary>
	public class OnSelfCast : IOnCastAction
	{
		[Required] [Tooltip("The actions that will be executed with the caster as target")] 
		public IOnHitAction[] OnHitActions = new IOnHitAction[0];
		
		private ModularSpell _owner;
		
		public void Init(ModularSpell owner)
		{
			_owner = owner;

			// Init OnHitActions
			foreach (IOnHitAction onHitAction in OnHitActions)
			{
				onHitAction.Init(owner);
			}
			
			owner.AddOnCastAction(this);
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			foreach (IOnHitAction onHitAction in OnHitActions)
			{
				onHitAction.OnHit(_owner.owner, castDirection, movementDirection);
			}
		}
	}
}