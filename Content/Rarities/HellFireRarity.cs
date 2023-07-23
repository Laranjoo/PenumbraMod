using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Rarities
{
	public class HellFireRarity : ModRarity
	{
		public override Color RarityColor => new Color(255, 157, 48);

		public override int GetPrefixedRarity(int offset, float valueMult) {
			if (offset > 2) { // If the offset is 1 or 2 (a positive modifier).
				return ModContent.RarityType<HellFireHigherRarity>(); // Make the rarity of items that have this rarity with a positive modifier the higher tier one.
			}

			return Type; // no 'lower' tier to go to, so return the type of this rarity.
		}
	}
}
