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
	public class BloodystoneOre : ModTile
	{
		public override void SetStaticDefaults() {
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
			Main.tileOreFinderPriority[Type] = 410; // Metal Detector value, see https://terraria.gamepedia.com/Metal_Detector
			Main.tileShine2[Type] = true; // Modifies the draw color slightly.
			Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Bloodystone Ore");
			AddMapEntry(new Color(177, 0, 0), name);

			DustType = 84;
			HitSound = SoundID.Tink;
			MineResist = 4f;
			MinPick = 200;
		}
	}

    public class BlodySystem : GlobalNPC
    {
        public static bool flag = false;
        public override void OnKill(NPC npc)
        {
            if (flag == false && (NPC.downedMechBoss3 || npc.type == NPCID.SkeletronPrime) && (NPC.downedMechBoss2 || npc.type == NPCID.TheDestroyer)
                && (NPC.downedMechBoss1 && (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)))
            {
                flag = true;
                for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 7E-05); k++)
                {
                    int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = WorldGen.genRand.Next((int)GenVars.rockLayer, Main.maxTilesY);
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (tile.HasTile && tile.TileType == TileID.Crimstone)
                    {
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(9, 12), WorldGen.genRand.Next(6, 8), ModContent.TileType<BloodystoneOre>());
                    }
                } 
            }
        }

    }
}
