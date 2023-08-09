using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Tiles
{
	public class Peridot : ModTile
	{
		public override void SetStaticDefaults() {
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
			Main.tileOreFinderPriority[Type] = 410; // Metal Detector value, see https://terraria.gamepedia.com/Metal_Detector
			Main.tileShine2[Type] = true; // Modifies the draw color slightly.
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(127, 178, 21), name);

			DustType = DustID.Stone;
			HitSound = SoundID.Tink;
			MineResist = 3f;
			MinPick = 30;
		}
	}
}
