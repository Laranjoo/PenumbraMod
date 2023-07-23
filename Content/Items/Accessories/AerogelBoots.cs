using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items.Accessories
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Shoes)]
	public class AerogelBoots : ModItem
	{
		public override void SetStaticDefaults() {
			/* Tooltip.SetDefault("8% increased movement speed"
				+ "\nGrants you to jump higher and fast fall"); */

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 28; // Width of the item
			Item.height = 28; // Height of the item
			Item.value = Item.sellPrice(silver: 5); // How many coins the item is worth
			Item.rare = ItemRarityID.Blue; // The rarity of the item
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player) {
			player.moveSpeed += 0.08f;
			player.jumpSpeedBoost = 4;
			player.maxFallSpeed *= 12;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AerogelBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}
