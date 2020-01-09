using System.Collections;
using GameActors;
using UnityEngine;

namespace Spells
{
	/// <summary>
	/// Moves the GameActor that casted this spell
	/// </summary>
	public class MoveCasterOnCast : IOnCastAction
	{
		[SuffixLabel("units", Overlay = true)]
		public float DistanceToMove;
		
		[SuffixLabel("seconds", Overlay = true)]
		public float MoveDuration;
		
		[Tooltip("Wether the movement input should be used for direction. Else the spell cast direction is used")]
		public bool UseMovementInput = true;
		
		[Tooltip("Wether the Moveing should go to the opposite direction")]
		public bool InOppositeDirection;

		private ModularSpell _owner;

		public void Init(ModularSpell owner)
		{
			_owner = owner;
			
			owner.AddOnCastAction(this);
		}


		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			if (UseMovementInput)
			{
				castDirection = movementDirection;
			}

			if (InOppositeDirection) castDirection *= -1;

			_owner.StartCoroutine(MoveActor(_owner.owner, castDirection));
		}
		
		
		/// <summary>
		/// Moves the actor
		/// </summary>
		/// <param name="actor"> GameActor to be moved</param>
		/// <param name="forwardVector"> Direction of the movement</param>
		/// <returns></returns>
		public IEnumerator MoveActor(GameActor actor, Vector3 forwardVector)
		{
			float startTime = Time.time;
			float elapsedTime = Time.time - startTime;
			float speed = DistanceToMove / MoveDuration;

			while (elapsedTime <= MoveDuration)
			{
				elapsedTime = Time.time - startTime;

				actor.Position += Time.deltaTime * speed * forwardVector;

				yield return null;
			}
		}
	}
}