using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items.Armors
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting X_Arms.png, X_Body.png and X_FemaleBody.png sprite-sheet files to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class BloodystoneCape : ModItem
	{
		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Bloodystone Cape");
			/* Tooltip.SetDefault("12% Increased Magical damage" +
                "\n6% reduced mana usage" +
                "\n20+ Max mana"
				+ "\nGrants you immunity to 'On Fire!' and Bleeding"); */
				

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Orange; // The rarity of the item
			Item.defense = 18; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
			player.buffImmune[BuffID.OnFire] = true; // Make the player immune to Fire
			player.GetDamage(DamageClass.Magic) += 0.12f;
			player.buffImmune[BuffID.Bleeding] = true;
			player.manaCost -= 0.06f;
			player.statManaMax2 += 20;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() 
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodystoneBar>(), 16);
			recipe.AddIngredient(ItemID.Bone, 5);
            recipe.AddIngredient(ItemID.Gi, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
	}
}
