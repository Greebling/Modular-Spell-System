using GameActors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class PlaySoundOnHit : IOnHitAction
	{
		[Required] public AudioSource AudioPlayer;
		public AudioClip[] Clips = new AudioClip[1];

		public bool PlayOnMaxRange;

		public void Init(ModularSpell owner)
		{
			owner.AddOnHitAction(this);
		}

		public void OnHit(GameActor actor, Vector3 castDirection, Vector3 movementDirection)
		{
			PlaySound();
		}

		public void OnMaxRange(Vector3 position, Vector3 castDirection, Vector3 movementDirection)
		{
			if (PlayOnMaxRange)
			{
				PlaySound();
			}
		}

		private void PlaySound()
		{
			AudioPlayer.clip = Clips[Random.Range(0, Clips.Length)];
			AudioPlayer.Play();
		}
	}
}