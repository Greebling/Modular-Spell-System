using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
	public class PlaySoundOnCast : IOnCastAction
	{
		[Required]
		public AudioSource AudioPlayer;
		public AudioClip[] Clips = new AudioClip[1];
		public void Init(ModularSpell owner)
		{
			owner.AddOnCastAction(this);
		}

		public void OnCast(Vector3 castDirection, Vector3 movementDirection)
		{
			AudioPlayer.clip = Clips[Random.Range(0, Clips.Length)];
			AudioPlayer.Play();
		}	
	}
}