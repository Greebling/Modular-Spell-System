using Core;
using UnityEngine;

namespace Spells
{
	public class CurvedProjectileMovement : IProjectileMovement
	{
		[Tooltip("The control points of the cubic bezier curve")]
		public Vector3[] curveControlPoints = new Vector3[4];

		private CubicBezier _bezierCurve;
		private float _startToEndDistance;

		/// <summary>
		/// Initialises the projectile with the given forward vector
		/// </summary>
		/// <param name="forwardVector"></param>
		public void OnProjectileStart(Vector3 forwardVector)
		{
			_bezierCurve = new CubicBezier(curveControlPoints);
			_startToEndDistance = Vector3.Distance(curveControlPoints[0], curveControlPoints[3]);
		}

		/// <summary>
		/// Returns the new forward vector of the projectile for a given traveled distance
		/// </summary>
		/// <param name="distance"></param>
		/// <returns></returns>
		public Vector3 CalculateForwardVector(double distance)
		{
			float t = (float) distance / _startToEndDistance;
			return _bezierCurve.F(t);
		}

		/// <summary>
		/// Returns the new rotation of the projectile for a given traveled distance
		/// </summary>
		/// <param name="distance"></param>
		/// <returns></returns>
		public Quaternion CalculateRotation(double distance)
		{
			float t = (float) distance / _startToEndDistance;
			Vector3 forwardVector = _bezierCurve.GetSpeed(t);
			return Quaternion.LookRotation(forwardVector);
		}

		/// <summary>
		/// Returns a copy of the IProjectileMovement that might not be initialised
		/// </summary>
		/// <returns></returns>
		public IProjectileMovement Clone()
		{
			CurvedProjectileMovement clonedMovement = new CurvedProjectileMovement();
			clonedMovement.curveControlPoints = curveControlPoints.Clone() as Vector3[];
			return clonedMovement;
		}
	}
}