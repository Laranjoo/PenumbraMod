using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Tiles
{
    public class GlacialChunkBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(166, 227, 239), name);
            DustType = DustID.Stone;
            HitSound = SoundID.Tink;
            MineResist = 5f;
            MinPick = 190;
        }
    }
}
