using GameActors;
using UnityEngine;

namespace Spells
{
	public interface IOnHitAction
	{
		void Init(ModularSpell owner);
		
		/// <summary>
		/// Called when a spell hits a gameActor. 
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="castDirection"></param>
		/// <param name="movementDirection"></param>
		void OnHit(GameActor actor, Vector3 castDirection, Vector3 movementDirection);
		
		/// <summary>
		/// Called when a projectile hits its maximum range
		/// </summary>
		/// <param name="position"></param>
		/// <param name="castDirection"></param>
		/// <param name="movementDirection"></param>
		void OnMaxRange(Vector3 position, Vector3 castDirection, Vector3 movementDirection);
	}
}