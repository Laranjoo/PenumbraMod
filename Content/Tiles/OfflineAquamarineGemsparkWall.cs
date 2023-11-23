using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Tiles
{
	public class OfflineAquamarineGemsparkWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;

			DustType = DustID.BlueTorch;
			AddMapEntry(new Color(9, 55, 88));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}