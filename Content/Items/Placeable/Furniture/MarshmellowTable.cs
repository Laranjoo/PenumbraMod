using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Consumables;

namespace PenumbraMod.Content.Items.Placeable.Furniture
{
	public class MarshmellowTable : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Marshmellow Table");
			// Tooltip.SetDefault("Makes your homies comfortable");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.MarshmellowTable>());
			Item.value = 150;
			Item.maxStack = 99;
			Item.width = 38;
			Item.height = 24;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Placeable.MarshmellowBlock>(8)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
