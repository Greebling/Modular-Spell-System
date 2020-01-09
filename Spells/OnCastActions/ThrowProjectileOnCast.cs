using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class ThrowProjectileOnCast : IOnCastAction
	{
		[Tooltip("If set to true the projectile will only hit the first enemy and then disappear")]
		public bool KillProjectileOnFirstHit;

		[SuffixLabel("units", Overlay = true)] public float ProjectileRange;
		[SuffixLabel("u/s", Overlay = true)] public float ProjectileSpeed;

		public bool useMovementInputForAim = false;

		[Required] public IProjectileMovement ProjectileMovement;
		[Required] public Projectile ProjectilePrefab;
		[Required] public IOnHitAction[] OnHitActions = new IOnHitAction[0];

		private ModularSpell _owner;
		public ModularSpell Spell => _owner;
		

		public void Init(ModularSpell owner)
		{
			_owner = owner;

			// Init OnHitActions
			foreach (IOnHitAction onHitAction in OnHitActions)
			{
				onHitAction.Init(owner);
			}
			
			owner.AddOnCastAction(this);
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			if (useMovementInputForAim)
			{
				castDirection = movementDirection;
			}
			
			// TODO: Object Pooling, also change KillProjectile() in Projectile as a result
			Projectile projectileInstance = Object.Instantiate(ProjectilePrefab.gameObject, _owner.transform.position,
				Quaternion.LookRotation(castDirection)).GetComponent<Projectile>();

			projectileInstance.movement = ProjectileMovement;
			projectileInstance.gameObject.layer = _owner.gameObject.layer;
			projectileInstance.owner = this;
			projectileInstance.OnProjectileStart(castDirection, movementDirection, ProjectileMovement.Clone());
		}
	}
}