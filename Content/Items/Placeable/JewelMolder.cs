using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Consumables;

namespace PenumbraMod.Content.Items.Placeable
{
	public class JewelMolder : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.JewelMolder>());
			Item.value = 150;
			Item.maxStack = 999;
			Item.width = 38;
			Item.height = 24;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.WorkBench)
				.AddIngredient(ItemID.StoneBlock, 25)
				.AddIngredient(ItemID.IronOre, 7)
				.AddIngredient(ItemID.Ruby, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
