using GameActors;
using UnityEngine;

namespace Spells
{
	public class PlayAnimationOnHit : IOnHitAction
	{
		[Tooltip("The name of the trigger that should be activated in the animator")]
		public string triggerName;

		[Tooltip("If set to false uses the animator of the hit GameActor, else it animates the casting GameActor")]
		public bool animateCaster;

		private GameActor _owner;
		private Animator _animator;

		public void Init(ModularSpell owner)
		{
			_owner = owner.owner;
			if (!animateCaster)
				_animator = _owner.GetComponent<Animator>();

			owner.AddOnHitAction(this);
		}

		public void OnHit(GameActor actor, Vector3 castDirection, Vector3 movementDirection)
		{
			if (animateCaster)
			{
				if (_animator)
				{
					_animator.SetTrigger(triggerName);
				}
			}
			else
			{
				Animator hitAnimator = actor.GetComponent<Animator>();

				if (hitAnimator)
					hitAnimator.SetTrigger(triggerName);
			}
		}

		public void OnMaxRange(Vector3 position, Vector3 castDirection, Vector3 movementDirection)
		{
		}
	}
}