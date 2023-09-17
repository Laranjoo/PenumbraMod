using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items.Consumables
{
	public class SoulHuntPotion : ModItem
	{
		public override void SetStaticDefaults() {
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
			Item.buffTime = 5 * 60 * 60;
			Item.buffType = ModContent.BuffType<SoulHunt>();

        }
        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient<EnchantedMushroom>(10)
				.AddIngredient(ItemID.Deathweed, 5)
				.AddIngredient(ItemID.Fireblossom, 5)
				.AddIngredient(ItemID.BottledWater, 1)
				.AddTile(TileID.Bottles)
				.Register();
        }
        public override bool ConsumeItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SoulHunt>(), 5 * 60 * 60);
            return true;
        }
    }
}
