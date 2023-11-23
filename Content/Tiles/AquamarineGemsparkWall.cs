using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Tiles
{
	public class AquamarineGemsparkWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;

			DustType = DustID.BlueTorch;
			AddMapEntry(new Color(108, 189, 248));
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.108f;
            g = 0.189f;
            b = 1.248f;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}