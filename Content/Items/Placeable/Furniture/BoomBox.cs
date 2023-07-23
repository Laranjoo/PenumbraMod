using PenumbraMod.Content.Items.Consumables;
using PenumbraMod.Content.Tiles;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Placeable.Furniture
{
	public class BoomBox : ModItem
	{
		public override void SetStaticDefaults() {
			// Tooltip.SetDefault("AOOOOBA");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/funk1"), ModContent.ItemType<BoomBox>(), ModContent.TileType<BoomBoxTile>());
        }

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.BoomBoxTile>());
			Item.value = 1500;
			Item.maxStack = 99;
			Item.width = 28;
			Item.height = 34;
			Item.rare = 2;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.IronBar, 25)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
