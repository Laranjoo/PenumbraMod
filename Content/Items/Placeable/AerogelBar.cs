using Microsoft.Xna.Framework;
using PenumbraMod.Content;
using PenumbraMod.Content.Rarities;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

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
        }
	}
}