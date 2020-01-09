using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class InstantiateObjectOnCast : IOnCastAction
	{
		[Required] public GameObject objectToInstantiate;

		[Tooltip("The scale of the instantiated Object")]
		public float instanceScale = 1;

		[Tooltip("If 0 the gameObject wont be automatically deleted. Else it will be deleted in the given seconds")]
		[SuffixLabel("seconds", Overlay = true)]
		public float objectLifeTime;

		[Tooltip("If set to true, the objects rotation will be set to the cast direction")]
		public bool setRotation = true;

		private Transform _ownerTransform;

		public void Init(ModularSpell owner)
		{
			_ownerTransform = owner.transform;

			owner.AddOnCastAction(this);
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			GameObject instantiatedGameObject;
			if (setRotation)
			{
				instantiatedGameObject = Object.Instantiate(objectToInstantiate, _ownerTransform.position,
					Quaternion.LookRotation(castDirection));
			}
			else
			{
				instantiatedGameObject = Object.Instantiate(objectToInstantiate, _ownerTransform.position,
					objectToInstantiate.transform.rotation);
			}

			instantiatedGameObject.transform.localScale *= instanceScale;
			
			if (objectLifeTime != 0)
			{
				Object.Destroy(instantiatedGameObject, objectLifeTime);
			}
		}
	}
}