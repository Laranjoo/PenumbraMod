using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Tiles
{
	public class RozeQuartzGemsparkWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;

			DustType = DustID.GreenTorch;
			AddMapEntry(new Color(248, 181, 243));
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1.248f;
            g = 0.181f;
            b = 0.243f;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}