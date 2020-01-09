using System.Collections;
using GameActors;
using UnityEngine;

namespace Spells
{
	public class MoveActorOnHit : IOnHitAction
	{
		public float DistanceToMove;
		public float MoveDuration;
		public bool InOppositeDirection;

		public bool MoveHitActor = true;
		public bool MoveCaster = false;

		private ModularSpell _owner;

		public void Init(ModularSpell owner)
		{
			_owner = owner;
			owner.AddOnHitAction(this);
		}

		public void OnHit(GameActor actor, Vector3 castDirection, Vector3 movementDirection)
		{
			if (InOppositeDirection) castDirection *= -1;

			if (MoveHitActor)
			{
				_owner.StartCoroutine(MoveActor(actor, castDirection));
			}

			if (MoveCaster)
			{
				_owner.StartCoroutine(MoveActor(_owner.owner, castDirection));
			}
		}

		public void OnMaxRange(Vector3 position, Vector3 castDirection, Vector3 movementDirection)
		{
		}

		public IEnumerator MoveActor(GameActor actor, Vector3 forwardVector)
		{
			float startTime = Time.time;
			float elapsedTime = Time.time - startTime;
			float speed = DistanceToMove / MoveDuration;

			while (elapsedTime <= MoveDuration)
			{
				elapsedTime = Time.time - startTime;

				if (actor)
				{
					actor.Position += Time.deltaTime * speed * forwardVector;
				}
				else
				{
					break;
				}

				yield return null;
			}
		}
	}
}