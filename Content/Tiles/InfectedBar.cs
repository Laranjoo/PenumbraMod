using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Tiles
{
    public class InfectedBar : ModTile
    {
        public override void SetStaticDefaults()
        {
            
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            
            AddMapEntry(new Color(182, 82, 221), Language.GetText("MapObject.InfectedBar")); // localized text for "Metal Bar"
        }
    }
}