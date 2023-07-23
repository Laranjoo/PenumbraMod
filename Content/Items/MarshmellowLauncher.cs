using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Consumables;

namespace PenumbraMod.Content.Items
{
	public class MarshmellowLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Marshmellow Launcher"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            // Tooltip.SetDefault("Fires fast yummy marshmellows");
			
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 60;
			Item.height = 32;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 1000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MarshmellowProjectile>();
			Item.useAmmo = ModContent.ItemType<Marshmellow>();
			Item.shootSpeed = 16f;
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
			
			recipe.AddIngredient(ItemID.SnowballCannon, 1);
			recipe.AddIngredient(ItemID.Bone, 15);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ModContent.ItemType<Marshmellow>(), 30);

            recipe.AddTile(TileID.Anvils);
            recipe.Register();
		}


	}
}