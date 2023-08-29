using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using PenumbraMod.Content.Buffs;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;

namespace PenumbraMod.Content.Items.Consumables
{
    public class BottledShimmer : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 30;
        }

        public override void SetDefaults()
        {
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
            Item.value = Item.buyPrice(gold: 1);
            Item.buffType = BuffID.PotionSickness;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(BuffID.PotionSickness);
        }
        public override bool ConsumeItem(Player player)
        {
            int heal = Main.rand.Next(80, 150);
            player.Heal(heal);
            player.AddBuff(BuffID.PotionSickness, heal * 22);
            player.AddBuff(Main.rand.Next(new int[]
                      {
                BuffID.Regeneration,
                BuffID.Ironskin,
                BuffID.Lucky,
                BuffID.MagicPower
                      }), 60 * 60);


            if (Main.rand.NextBool(11))
            {
                player.AddBuff(BuffID.Shimmer, Main.rand.Next(5 * 60));
                player.shimmerWet = true;
            }

            return true;
        }
    }
}
