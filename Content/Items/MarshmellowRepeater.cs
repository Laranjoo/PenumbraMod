using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Consumables;

namespace PenumbraMod.Content.Items
{
	public class MarshmellowRepeater : ModItem
	{
		public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 58;
			Item.height = 22;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 1000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MarshmellowGold>();
			Item.useAmmo = ModContent.ItemType<Marshmellow>();
			Item.shootSpeed = 22f;
			Item.noMelee = true;
			Item.crit = 14;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, 1f);
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			
			recipe.AddIngredient(ModContent.ItemType<MarshmellowLauncher>(), 1);
            recipe.AddIngredient(ModContent.ItemType<MarshmellowCannon>(), 1);
            recipe.AddIngredient(ItemID.GoldBar, 15);
            recipe.AddIngredient(ItemID.Gatligator, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
		}
	}
}