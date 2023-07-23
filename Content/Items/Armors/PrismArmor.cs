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
	public class PrismArmor : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Prism Breastplate");
			// Tooltip.SetDefault("5% Increased reaper damage");
            

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 28; // Width of the item
			Item.height = 24; // Height of the item
			Item.value = Item.sellPrice(gold: 2); // How many coins the item is worth
			Item.rare = ItemRarityID.LightPurple; // The rarity of the item
			Item.defense = 19; // The amount of defense the item will give when equipped
			

        }

		public override void UpdateEquip(Player player) {
			player.GetDamage<ReaperClass>() += 0.05f;
			
		}
	}
    
}
