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
    public class InfectedOre : ModTile
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
            // name.SetDefault("Infected Ore");
            AddMapEntry(new Color(183, 82, 221), name);

            DustType = 84;
            HitSound = SoundID.Tink;
            MineResist = 4f;
            MinPick = 200;
        }
    }
    public class InfectedSystem : GlobalNPC
    {
        //pseudocode
        //if(!flag && (downedmech3 || prime) && (downedmech2 || skeletron) && (downedmech1 && (retinazer || spaztism)){ 
        //  flag = true;
        // spawn bosses
        public static bool flag = false;
        public override void OnKill(NPC npc)
        {
            if (flag == false && (NPC.downedMechBoss3 || npc.type == NPCID.SkeletronPrime) && (NPC.downedMechBoss2 || npc.type == NPCID.TheDestroyer) 
                && (NPC.downedMechBoss1 && (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)))
            {
                flag = true;
                Main.NewText("[c/00ffdf:A new stone has appeared on the evil lands!]");
                for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 7E-05); k++)
                {
                    int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = WorldGen.genRand.Next((int)GenVars.rockLayer, Main.maxTilesY);
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (tile.HasTile && tile.TileType == TileID.Ebonstone)
                    {
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(9, 12), WorldGen.genRand.Next(6, 8), ModContent.TileType<InfectedOre>());
                    }
                }
            }
        }

    }
}
