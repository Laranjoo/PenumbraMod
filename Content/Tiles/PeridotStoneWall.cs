using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Tiles
{
	public class PeridotStoneWall : ModWall
	{
		public override void SetStaticDefaults() {
			DustType = DustID.GreenTorch;
			AddMapEntry(new Color(12, 33, 6));
		}
        public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}