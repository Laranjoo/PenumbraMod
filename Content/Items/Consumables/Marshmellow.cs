using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Consumables
{
	public class Marshmellow : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 30;
		}
        public override void SetDefaults() {
			Item.width = 8;
			Item.height = 8;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item2;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(copper: 50);
            Item.ammo = Item.type;
        }

        public override bool ConsumeItem(Player player)
        {
            player.AddBuff(BuffID.WellFed, 12000);
            return true;
        }
    }
}