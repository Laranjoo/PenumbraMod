using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Rarities
{
	public class HellFireHigherRarity : ModRarity
	{
		public override Color RarityColor => new Color(255, 170, 52);

		public override int GetPrefixedRarity(int offset, float valueMult) {
			if (offset < 0) { // If the offset is -1 or -2 (a negative modifier).
				return ModContent.RarityType<HellFireRarity>(); // Make the rarity of items that have this rarity with a negative modifier the lower tier one.
			}

			return Type; // no 'higher' tier to go to, so return the type of this rarity.
		}
	}
}
