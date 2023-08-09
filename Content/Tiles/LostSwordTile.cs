using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.Items;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PenumbraMod.Content.Tiles
{
	public class LostSwordTile : ModTile
	{

		public override void SetStaticDefaults()
		{
			// Properties
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			DustType = 1;
			
			// Names
			AddMapEntry(new Color(67, 67, 67), Language.GetText("Lost Sword"));

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.addTile(Type);
        }

        public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, ModContent.ItemType<RuneBlade>());
		}
	}
}
