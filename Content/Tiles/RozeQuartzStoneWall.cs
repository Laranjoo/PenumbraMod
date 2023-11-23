using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Tiles
{
	public class RozeQuartzStoneWall : ModWall
	{
		public override void SetStaticDefaults() {
			DustType = DustID.PurpleTorch;
			AddMapEntry(new Color(12, 33, 6));
		}
        public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}