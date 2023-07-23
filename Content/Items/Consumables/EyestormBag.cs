using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;

namespace PenumbraMod.Content.Items.Consumables
{
	
	public class EyestormBag : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Eye of the Storm Treasure Bag");
			/* Tooltip.SetDefault("The storm rests..."
				+ "\n{$CommonItemTooltip.RightClickToOpen}"); */ // References a language key that says "Right Click To Open" in the language of the game

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults() {
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.expert = true;
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot) {
            itemLoot.Add(ItemDropRule.Common(ItemID.GoldCoin, 1, 1, 2));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShockWave>(), 3, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SparkBow>(), 3, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StaffofEnergy>(), 3, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ChargeGun>(), 3, 1));
            itemLoot.Add(ItemDropRule.Common(ItemID.SandBoots, 7, 1));
            itemLoot.Add(ItemDropRule.Common(ItemID.SandBlock, 1, 15, 30));
            itemLoot.Add(ItemDropRule.Common(ItemID.GoldBar, 1, 5, 15));
        }

		
	}
}
