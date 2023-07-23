using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Consumables;

namespace PenumbraMod.Content.Items.Placeable.Furniture
{
	public class MarshmellowWorkbench : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Marshmellow Work Bench");
			// Tooltip.SetDefault("");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.createTile = ModContent.TileType<Tiles.MarshmellowWorkbench>(); // This sets the id of the tile that this item should place when used.

			Item.width = 28; // The item texture's width
			Item.height = 14; // The item texture's height

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.maxStack = 99;
			Item.consumable = true;
			Item.value = 150;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Marshmellow>(10)
				.Register();
		}
	}
}
