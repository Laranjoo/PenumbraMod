using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.DamageClasses;

namespace PenumbraMod.Content.Items.Armors
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting X_Arms.png, X_Body.png and X_FemaleBody.png sprite-sheet files to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class PerishedRobe : ModItem
	{
		public override void SetStaticDefaults() {
		}

		public override void SetDefaults() {
			Item.width = 28; // Width of the item
			Item.height = 24; // Height of the item
			Item.value = Item.sellPrice(copper: 30); // How many coins the item is worth
			Item.rare = ItemRarityID.Blue; // The rarity of the item
			Item.defense = 3; // The amount of defense the item will give when equipped
			

        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Silk, 10)
            .AddIngredient(ItemID.Rope, 7)
            .AddIngredient(ItemID.ViciousPowder, 5)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
        public override void UpdateEquip(Player player) {
			player.GetAttackSpeed<ReaperClass>() += 0.05f;
			
		}
	}
    
}
