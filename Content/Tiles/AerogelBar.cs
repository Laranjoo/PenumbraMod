using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Tiles
{
    public class AerogelBar : ModTile
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

            
            AddMapEntry(new Color(146, 103, 255), Language.GetText("MapObject.AerogelBar")); // localized text for "Metal Bar"
        }

        public override bool CanDrop(int i, int j)/* tModPorter Note: Removed. Use CanDrop to decide if an item should drop. Use GetItemDrops to decide which item drops. Item drops based on placeStyle are handled automatically now, so this method might be able to be removed altogether. */
        {
            Tile t = Main.tile[i, j];
            int style = t.TileFrameX / 18;

            // It can be useful to share a single tile with multiple styles. This code will let you drop the appropriate bar if you had multiple.
            if (style == 0)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Placeable.AerogelBar>());
            }

            return base.CanDrop(i, j);
        }
    }
}