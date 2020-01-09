using GameActors;
using UnityEngine;

namespace Spells
{
	public class PlayAnimationOnCast : IOnCastAction
	{
		[Tooltip("The name of the trigger that should be activated in the animator")]
		public string triggerName;
		
		private GameActor _owner;
		private Animator _animator;
		
		public void Init(ModularSpell owner)
		{
			_owner = owner.owner;
			_animator = _owner.GetComponent<Animator>();
			
			owner.AddOnCastAction(this);
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			if (_animator)
			{
				_animator.SetTrigger(triggerName);
			}
		}
	}
}