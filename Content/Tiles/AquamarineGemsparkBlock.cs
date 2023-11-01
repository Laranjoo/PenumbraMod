using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Tiles
{
    public class AquamarineGemsparkBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            //TileID.Sets.GemsparkFramingTypes[Type] = Type; this glitched out the framing my guy :sob:

            DustType = ModContent.DustType<Dusts.IndigoTorch>();
            AddMapEntry(new Color(108, 189, 248));
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
            return false;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.108f;
            g = 0.189f;
            b = 1.248f;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
