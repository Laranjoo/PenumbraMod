using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Tiles
{
	public class MarshmellowWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;

			DustType = 1;
			AddMapEntry(new Color(180, 180, 180));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}