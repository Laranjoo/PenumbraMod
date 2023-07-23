using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using PenumbraMod.Content.Buffs;

namespace PenumbraMod.Content.Items.Consumables
{
	public class JumpVelPotion : ModItem
	{
		public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Frog Potion");
			// Tooltip.SetDefault("Increases your speed and jump");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 30;

			
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = 30;
			Item.consumable = true;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.buyPrice(gold: 2);
			Item.buffTime = 18000;
			Item.buffType = ModContent.BuffType<JumpVelBuff>();

        }

        public override bool ConsumeItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<JumpVelBuff>(), 18000);
            return true;
        }
    }
}
