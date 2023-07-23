using PenumbraMod.Content.Items.Consumables;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Placeable.Furniture
{
	public class Narga : ModItem
	{
		public override void SetStaticDefaults() {
			/* Tooltip.SetDefault("If you use it..." +
				"\nYou shall enter in the world of craziness!!" +
				"\nIts cool..."); */

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Narga>());
			Item.value = 1500;
			Item.maxStack = 99;
			Item.width = 28;
			Item.height = 34;
			Item.rare = 2;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.IronBar, 12)
				.AddIngredient(ItemID.Glass, 8)
				.AddIngredient(ItemID.StrangeBrew, 1)
				.AddIngredient(ItemID.Mushroom, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
