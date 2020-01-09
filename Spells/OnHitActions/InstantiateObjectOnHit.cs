using GameActors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class InstantiateObjectOnHit : IOnHitAction
	{
		[Required] public GameObject objectToInstantiate;
		
		[SuffixLabel("seconds", Overlay = true)]
		public float objectLifeTime;
		public bool alsoInstantiateOnMaxRange;

		public void Init(ModularSpell owner)
		{
			owner.AddOnHitAction(this);
		}

		public void OnHit(GameActor actor, Vector3 castDirection, Vector3 movementDirection)
		{
			InstantiateObject(actor.MidPosition.position, castDirection);
		}

		private void InstantiateObject(Vector3 position, Vector3 forwardVector)
		{
			GameObject instantiatedGameObject = Object.Instantiate(objectToInstantiate, position,
				Quaternion.LookRotation(forwardVector));

			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (objectLifeTime != 0)
			{
				Object.Destroy(instantiatedGameObject, objectLifeTime);
			}
		}

		public void OnMaxRange(Vector3 position, Vector3 castDirection, Vector3 movementDirection)
		{
			if (alsoInstantiateOnMaxRange)
			{
				InstantiateObject(position, castDirection);
			}
		}
	}
}