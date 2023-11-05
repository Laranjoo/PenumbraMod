using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PenumbraMod.Content.Tiles
{
	public class Narga : ModTile
	{

		public override void SetStaticDefaults()
		{
			// Properties
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;


			DustType = 1;
			
			// Names
			AddMapEntry(new Color(67, 67, 67), Language.GetText("Narga"));

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.addTile(Type);
        }

        public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
        public override bool RightClick(int i, int j) {
			Player player = Main.LocalPlayer;

			Main.LocalPlayer.AddBuff(ModContent.BuffType<BlackLungs1>(), 60000);
            SoundEngine.PlaySound(SoundID.SplashWeak);
			SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/Player/PlayerCough")
			{
				Volume = 1.6f,
				PitchVariance = 0.2f,
				MaxInstances = 1,
				PlayOnlyIfFocused = true,
				
			});
            return true; 
		}

		public override void MouseOver(int i, int j) {
			Player player = Main.LocalPlayer;

			
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.Narga>();

			if (Main.tile[i, j].TileFrameX / 18 < 1) {
				player.cursorItemIconReversed = true;
			}
		}
	}
}
