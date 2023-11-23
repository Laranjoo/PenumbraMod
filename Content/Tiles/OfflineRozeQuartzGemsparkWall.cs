using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Tiles
{
	public class OfflineRozeQuartzGemsparkWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;

			DustType = DustID.PinkTorch;
			AddMapEntry(new Color(124, 35, 153));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}