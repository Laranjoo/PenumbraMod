using PenumbraMod.Content.Items.Consumables;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Placeable.Furniture
{
	public class MarshmellowChair : ModItem
	{
		public override void SetStaticDefaults() {
			// Tooltip.SetDefault("Sitting on this comforts you...");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.MarshmellowChair>());
			Item.value = 150;
			Item.maxStack = 99;
			Item.width = 12;
			Item.height = 30;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Placeable.MarshmellowBlock>(5)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
