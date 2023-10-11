using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace PenumbraMod.Content.Tiles
{
    public class InfectedOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
            Main.tileOreFinderPriority[Type] = 410; // Metal Detector value, see https://terraria.gamepedia.com/Metal_Detector
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Infected Ore");
            AddMapEntry(new Color(183, 82, 221), name);

            DustType = DustID.PurpleMoss;
            HitSound = SoundID.Tink;
            MineResist = 4f;
            MinPick = 190;
        }
    }
    public class InfectedSystem : GlobalNPC
    {   
        public override void OnKill(NPC npc)
        {
           /* if (Main.hardMode || npc.type == NPCID.WallofFlesh)
            {
                Main.NewText("[c/00ffdf:A new stone has appeared on the evil lands!]");
                for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 8E-05); k++)
                {
                    int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = WorldGen.genRand.Next((int)GenVars.rockLayer, Main.maxTilesY);
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (tile.HasTile && tile.TileType == TileID.Ebonstone)
                    {
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(10, 12), WorldGen.genRand.Next(10, 12), ModContent.TileType<InfectedOre>());
                    }
                }
            }   */
        }

    }
}
