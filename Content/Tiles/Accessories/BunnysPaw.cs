using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using PenumbraMod.Content.Items.Placeable;
using PenumbraMod.Common.Players;

namespace PenumbraMod.Content.Items.Accessories
{
	public class BunnysPaw : ModItem
	{
		public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Lucky Bunny Foot");
            /* Tooltip.SetDefault("5% Increased Critical strike chance"
				+ "\nHighers your luck a bit" +
				"\n''You are so mean!''"); */

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 22; // Width of the item
			Item.height = 22; // Height of the item
			Item.value = Item.sellPrice(gold: 1 ,silver: 5, copper: 45); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player) {
            player.GetCritChance(DamageClass.Generic) += 5f;
			player.GetModPlayer<LuckSys>().bunny = true;
        }
	}
}
