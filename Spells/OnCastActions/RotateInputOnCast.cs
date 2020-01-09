using UnityEngine;

namespace Spells
{
	/// <summary>
	/// Rotates the castDirection a certain amount of degrees around the global Y-Axis
	/// </summary>
	public class RotateInputOnCast : IOnCastAction
	{
		[Tooltip("Amount in degrees the input should be rotated around the global Y-Axis")]
		public float rotationAmount;

		[Tooltip("If true, rotates the aim input rotationAmount degrees on cast")]
		public bool rotateAimInput;
		[Tooltip("If true, rotates the movement input rotationAmount degrees on cast")]
		public bool rotateMovementInput;
		
		[Tooltip("The IOnCastActions that shall use the rotated Inputs")]
		public IOnCastAction[] castActions = new IOnCastAction[0];

		public void Init(ModularSpell owner)
		{
			owner.AddOnCastAction(this);

			// Init castActions
			foreach (IOnCastAction onCastAction in castActions)
			{
				onCastAction.Init(owner);
			}
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			if (rotateAimInput)
				castDirection = RotateVectorAroundY(castDirection, rotationAmount);

			if (rotateMovementInput)
				movementDirection = RotateVectorAroundY(movementDirection, rotationAmount);

			foreach (IOnCastAction onCastAction in castActions)
			{
				onCastAction.OnCast(castDirection, movementDirection);
			}
		}

		/// <summary>
		/// Rotates a given Vector yRotation degrees around the global Y-Axis
		/// </summary>
		/// <param name="inputVector"> The direction the spell is cast</param>
		/// <param name="yRotation"> The amount the castDirection shal be rotated in degrees</param>
		/// <returns></returns>
		private Vector3 RotateVectorAroundY(Vector3 inputVector, float yRotation)
		{
			Quaternion quaternionFromVector = Quaternion.LookRotation(inputVector);

			inputVector = quaternionFromVector.eulerAngles;
			inputVector.y += yRotation;
			quaternionFromVector = Quaternion.Euler(inputVector);
			inputVector = quaternionFromVector * Vector3.forward;

			return inputVector;
		}
	}
}