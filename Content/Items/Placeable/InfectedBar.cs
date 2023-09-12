using Microsoft.Xna.Framework;
using PenumbraMod.Content;
using PenumbraMod.Content.Rarities;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Placeable
{
    public class InfectedBar : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.InfectedBar>();
            Item.width = 12;
            Item.height = 12;
            Item.value = 27000;
            Item.rare = 4;
        }
        
        public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DemoniteBar, 2);
            recipe.AddIngredient(ModContent.ItemType<Content.Items.Placeable.InfectedOre>(), 4);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.Register();
        }
	}
}