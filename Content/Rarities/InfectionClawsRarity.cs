using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Rarities
{
	public class InfectionClawsRarity : ModRarity
	{
		public override Color RarityColor => new Color(146, 125, 132);

		public override int GetPrefixedRarity(int offset, float valueMult) {
			if (offset > 2) { // If the offset is 1 or 2 (a positive modifier).
				return ModContent.RarityType<InfectionClawsHigherRarity>(); // Make the rarity of items that have this rarity with a positive modifier the higher tier one.
			}

			return Type; // no 'lower' tier to go to, so return the type of this rarity.
		}
	}
}
