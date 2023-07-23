using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content;
using PenumbraMod.Content.Items.Consumables;

namespace PenumbraMod.Content.Items
{
	public class MarshmellowCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 46;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 1000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MarshmellowSlowProjectile>();
			Item.useAmmo = ModContent.ItemType<Marshmellow>();
			Item.shootSpeed = 12f;
			Item.noMelee = true;
			Item.crit = 6;
		}


        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 8);
			recipe.AddIngredient(ModContent.ItemType<Consumables.Marshmellow>(), 7);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
		}


	}
}