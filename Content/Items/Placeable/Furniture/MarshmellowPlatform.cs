using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Consumables;

namespace PenumbraMod.Content.Items.Placeable.Furniture
{
	public class MarshmellowPlatform : ModItem
	{
		public override void SetStaticDefaults() {
			// Tooltip.SetDefault("");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 200;
		}

		public override void SetDefaults() {
			Item.width = 8;
			Item.height = 10;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.MarshmellowPlatform>();
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe(2)
				.AddIngredient(ModContent.ItemType<Tiles.MarshmellowBlock>(), 2)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}