using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using PenumbraMod.Content.Items.Placeable;
using PenumbraMod.Content.Items.Consumables;

namespace PenumbraMod.Content.Tiles
{ 
    public class MarshmellowBlock : ModItem
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
            Item.createTile = ModContent.TileType<Items.Placeable.MarshmellowBlock>();
            Item.width = 12;
            Item.height = 12;
            Item.value = 1000;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Marshmellow>(), 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<Items.Placeable.Furniture.MarshmellowWall>(), 4);
            recipe2.AddTile(TileID.WorkBenches);
            recipe2.Register();

            Recipe recipe3 = CreateRecipe();
            recipe3.AddIngredient(ModContent.ItemType<Items.Placeable.Furniture.MarshmellowPlatform>(), 2);
            recipe3.AddTile(TileID.WorkBenches);
            recipe3.Register();
        }
    }
}