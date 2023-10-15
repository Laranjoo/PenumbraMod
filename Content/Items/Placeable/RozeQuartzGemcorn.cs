using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Placeable
{
	public class RozeQuartzGemcorn: ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true; // This prevents this item from being automatically dropped from ExampleHerb tile. 
			Item.ResearchUnlockCount = 25;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Plants.RozeQuartzTreeSapling>(), 0);
			Item.width = 12;
			Item.height = 14;
			Item.value = 1400;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.Acorn)
				.AddIngredient(ModContent.ItemType<RozeQuartz>())
				.Register();
		}
	}
}
