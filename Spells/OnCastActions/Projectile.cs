using System.Linq;
using C;
using GameActors;
using UnityEngine;

namespace Spells
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Collider))]
	public class Projectile : MonoBehaviour
	{
		public ThrowProjectileOnCast owner;
		public IProjectileMovement movement;

		private Rigidbody _rb;
		private double _traveledDistance;
		private Vector3 _movementDirection;

		/// <summary>
		/// Initialises the projectile with a given cast direction and movement direction for OnHitActions
		/// and a ProjectileMovement Behaviour
		/// </summary>
		/// <param name="castDirection"></param>
		/// <param name="movementDirection"></param>
		/// <param name="projectileMovement"></param>
		public void OnProjectileStart(Vector3 castDirection, Vector3 movementDirection,
			IProjectileMovement projectileMovement)
		{
			movement = projectileMovement;
			movement.OnProjectileStart(castDirection);


			_rb = GetComponent<Rigidbody>();
			_movementDirection = movementDirection;

			
			// We use OnTriggerEnter for OnHit Effects, so make sure the collider is a trigger
			bool hasTriggerCollider = false;
			foreach (Collider col in GetComponents<Collider>())
			{
				if (col.isTrigger)
				{
					hasTriggerCollider = true;
					break;
				}
			}
			Debug.Assert(hasTriggerCollider, "Object needs a Collider Component that is a trigger");
		}

		private void FixedUpdate()
		{
			// Check if we hit max range
			if (owner.ProjectileRange <= _traveledDistance)
			{
				KillProjectile();
			}

			// move forward and set rotation
			_traveledDistance += owner.ProjectileSpeed * Time.deltaTime;
			_rb.velocity = owner.ProjectileSpeed * movement.CalculateForwardVector(_traveledDistance);
			_rb.rotation = movement.CalculateRotation(_traveledDistance);
		}

		/// <summary>
		/// Destroys the projectiles gameobject and calls IOnHitActions OnMaxRange beforehand
		/// </summary>
		private void KillProjectile()
		{
			foreach (IOnHitAction ownerOnHitAction in owner.OnHitActions)
			{
				ownerOnHitAction.OnMaxRange(_rb.position, movement.CalculateForwardVector(owner.ProjectileRange),
					_movementDirection);
			}

			Destroy(gameObject);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag(Tags.ENEMY) && !other.CompareTag(Tags.PLAYER)) return;

			GameActor hitActor = other.GetComponent<GameActor>();
			if (!hitActor || !owner.Spell.teamsToHit.Contains(hitActor.Side)) return;

			// Execute the OnHitActions
			foreach (IOnHitAction ownerOnHitAction in owner.OnHitActions)
			{
				ownerOnHitAction.OnHit(hitActor, transform.forward, _movementDirection);
			}

			if (owner.KillProjectileOnFirstHit)
			{
				Destroy(gameObject);
			}
		}
	}
}