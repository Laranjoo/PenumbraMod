using Microsoft.Xna.Framework;
using PenumbraMod.Content;
using PenumbraMod.Content.Rarities;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace PenumbraMod.Content.Items.Placeable
{
    public class InfectedOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.InfectedOre>();
            Item.width = 12;
            Item.height = 12;
            Item.value = 3000;
            Item.rare = 4;
        }

    }
    public class InfectedSystem : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (Main.hardMode || npc.type == NPCID.WallofFlesh)
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
            }
        }
    }
}