using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items.Placeable
{
    public class BloodyfleshBlock : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.BloodyfleshBlock>();
            Item.width = 12;
            Item.height = 12;
            Item.value = 3000;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
           
            recipe.AddIngredient(ModContent.ItemType<Content.Items.Placeable.BloodystoneOre>(), 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}