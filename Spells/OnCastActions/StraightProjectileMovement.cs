using UnityEngine;

namespace Spells
{
	public class StraightProjectileMovement : IProjectileMovement
	{
		private Vector3 _direction;
		private Quaternion _forwardRotation;
		
		/// <summary>
		/// Initialises the projectile with the given forward vector
		/// </summary>
		/// <param name="forwardVector"></param>
		public void OnProjectileStart(Vector3 forwardVector)
		{
			_direction = forwardVector;

			_forwardRotation = Quaternion.LookRotation(forwardVector);
		}

		/// <summary>
		/// Returns the new forward vector of the projectile for a given traveled distance
		/// </summary>
		/// <param name="distance"></param>
		/// <returns></returns>
		public Vector3 CalculateForwardVector(double distance)
		{
			return _direction;
		}

		/// <summary>
		/// Returns the new rotation of the projectile for a given traveled distance
		/// </summary>
		/// <param name="distance"></param>
		/// <returns></returns>
		public Quaternion CalculateRotation(double distance)
		{
			return _forwardRotation;
		}

		/// <summary>
		/// Returns a copy of the IProjectileMovement that might not be initialised
		/// </summary>
		/// <returns></returns>
		public IProjectileMovement Clone()
		{
			StraightProjectileMovement clonedMovement = new StraightProjectileMovement();
			return clonedMovement;
		}
	}
}