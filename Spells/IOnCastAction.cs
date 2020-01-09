using UnityEngine;

namespace Spells
{
	public interface IOnCastAction
	{
		/// <summary>
		/// Gets called at the pickup of the modular spell owner. All initiation should be done here. 
		/// </summary>
		/// <param name="owner"></param>
		void Init(ModularSpell owner);

		/// <summary>
		/// Executes the IOnCastAction. castDirection and movementDirection are the players joystick input ranging from -1 to 1
		/// </summary>
		/// <param name="castDirection"></param>
		/// <param name="movementDirection"></param>
		void OnCast(Vector3 castDirection, Vector3 movementDirection);
	}
}