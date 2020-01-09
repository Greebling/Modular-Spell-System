using UnityEngine;

namespace Spells
{
	public interface IProjectileMovement
	{
		/// <summary>
		/// Initialises the projectile with the given forward vector
		/// </summary>
		/// <param name="forwardVector"></param>
		void OnProjectileStart(Vector3 forwardVector);

		/// <summary>
		/// Returns the new forward vector of the projectile for a given traveled distance
		/// </summary>
		/// <param name="distance"></param>
		/// <returns></returns>
		Vector3 CalculateForwardVector(double distance);

		/// <summary>
		/// Returns the new rotation of the projectile for a given traveled distance
		/// </summary>
		/// <param name="distance"></param>
		/// <returns></returns>
		Quaternion CalculateRotation(double distance);

		/// <summary>
		/// Returns a copy of the IProjectileMovement that might not be initialised
		/// </summary>
		/// <returns></returns>
		IProjectileMovement Clone();
	}
}