using Player;
using Sirenix.OdinInspector;
using Spells;

namespace Artifacts
{
	public abstract class Artifact : SerializedMonoBehaviour
	{
		public abstract string ArtifactName { get;  }
		public abstract void OnPlayerAdd(PlayerController player);
		public abstract void OnSpellAdd(ModularSpell addedSpell);
	}
}