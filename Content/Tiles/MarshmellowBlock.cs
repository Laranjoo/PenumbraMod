using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items.Placeable
{
    public class MarshmellowBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;

            DustType = 1;
          
            AddMapEntry(new Color(235, 235, 235));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}  
 