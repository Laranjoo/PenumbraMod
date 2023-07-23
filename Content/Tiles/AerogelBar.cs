using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items.Placeable
{
    public class AerogelBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("AerogelBar");
            // Tooltip.SetDefault("This sticky bar looks tasty!");
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
            Item.createTile = ModContent.TileType<Tiles.AerogelBar>();
            Item.width = 12;
            Item.height = 12;
            Item.value = 3000;
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Gel, 15);
            recipe.AddIngredient(ItemID.GoldBar, 1);
            recipe.AddIngredient<InconsistentJelly>(2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.Gel, 15);
            recipe2.AddIngredient(ItemID.PlatinumBar, 1);
            recipe2.AddIngredient<InconsistentJelly>(2);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }
    }
}
