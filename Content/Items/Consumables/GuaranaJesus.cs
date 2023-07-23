using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using CsvHelper.TypeConversion;

namespace PenumbraMod.Content.Items.Consumables
{
	public class GuaranaJesus : ModItem
	{
		public override void SetStaticDefaults() {
			/* Tooltip.SetDefault("2 Dollars each"
				+ "\n'Tastes like gum!'"
				+ "\nGrants a bunch of buffs"); */

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

			
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
			Item.value = Item.buyPrice(platinum: 5);
			Item.buffType =  BuffID.WellFed3; // Specify an existing buff to be applied when used.
            Item.buffTime = 57600; // The amount of time the buff declared in Item.buffType will last in ticks. 5400 / 60 is 90, so this buff will last 90 seconds.
		}

        public override bool ConsumeItem(Player player)
        {
            player.AddBuff(BuffID.Ironskin, 57600);
            player.AddBuff(BuffID.Endurance, 57600);
			player.AddBuff(BuffID.Sunflower, 57600);
			player.AddBuff(BuffID.Heartreach, 57600);
            player.AddBuff(BuffID.Honey, 57600);
            player.AddBuff(BuffID.Shine, 57600);
            player.AddBuff(BuffID.NightOwl, 57600);
            player.AddBuff(BuffID.DryadsWard, 57600);
            player.AddBuff(BuffID.Archery, 57600);
            player.AddBuff(BuffID.Rage, 57600);
            player.AddBuff(BuffID.Wrath, 57600);
            player.AddBuff(BuffID.Lucky, 57600);
            player.AddBuff(BuffID.EmpressBlade, 3600);
            player.AddBuff(BuffID.Lifeforce, 57600);
            player.AddBuff(BuffID.CatBast, 57600);
            return true;
        }
    }
}
