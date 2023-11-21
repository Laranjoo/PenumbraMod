using PenumbraMod.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace PenumbraMod.Common.GlobalNPCs
{
    public class PostMechsOresSpawnSystem : GlobalNPC
    {
        // Code totally inspired by Calamity, i know it looks exactly the same but i didnt knew an other way to do that, sorry cal devs :(
        public override void OnKill(NPC npc)
        {
            bool checkTheLastTwinAlive = false;
            if (npc.type == NPCID.Retinazer)
                checkTheLastTwinAlive = !NPC.AnyNPCs(NPCID.Spazmatism);
            else if (npc.type == NPCID.Spazmatism)
                checkTheLastTwinAlive = !NPC.AnyNPCs(NPCID.Retinazer);
            switch (npc.type)
            {
                case NPCID.TheDestroyer:
                    if (!NPC.downedMechBoss1)
                        SpawnPostMechsOres();
                    break;

                case NPCID.Spazmatism:
                case NPCID.Retinazer:
                    if (checkTheLastTwinAlive)
                    {
                        if (!NPC.downedMechBoss2)
                            SpawnPostMechsOres();
                    }
                    break;
                case NPCID.SkeletronPrime:
                    if (!NPC.downedMechBoss3)
                        SpawnPostMechsOres();
                    break;
            }
        }
        void SpawnPostMechsOres()
        {
            if ((!NPC.downedMechBoss1 && !NPC.downedMechBoss2) || (!NPC.downedMechBoss2 && !NPC.downedMechBoss3) || (!NPC.downedMechBoss3 && !NPC.downedMechBoss1))
            {

            }
            else
            {
                Main.NewText((string)LocalizedTextForPostMechsOres.Text);
                for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 12E-05); k++)
                {
                    int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = WorldGen.genRand.Next((int)GenVars.rockLayer, Main.maxTilesY);
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (tile.HasTile && tile.TileType == TileID.Ebonstone)
                        WorldGen.OreRunner(x, y, WorldGen.genRand.Next(6, 12), WorldGen.genRand.Next(10, 12), (ushort)ModContent.TileType<InfectedOre>());
                    if (tile.HasTile && tile.TileType == TileID.Crimstone)
                        WorldGen.OreRunner(x, y, WorldGen.genRand.Next(6, 12), WorldGen.genRand.Next(10, 12), (ushort)ModContent.TileType<BloodystoneOre>());
                }
            }
        }

    }
    class LocalizedTextForPostMechsOres : ModSystem
    {
        public static LocalizedText Text { get; private set; }
        public override void Load()
        {
            string category = "PostMechsOres";
            Text ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Text"));
        }
    }
}
