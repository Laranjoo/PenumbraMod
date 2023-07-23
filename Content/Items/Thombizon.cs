using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Consumables;
using Terraria.Audio;

namespace PenumbraMod.Content.Items
{
	public class Thombizon : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Thombizon"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("Five round burst" +
				"\nOnly the first shot consumes ammo" +
				"\n''Made with love!''"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 23;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 60;
			Item.height = 32;
			Item.useTime = 4;
			Item.useAnimation = 18;
			Item.reuseDelay = 10;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.value = 4450;
			Item.rare = 3;
            Item.UseSound = new SoundStyle("PenumbraMod/Assets/Sounds/Items/Thombizon")
            {
                Volume = 1.0f,
                PitchVariance = 0.3f,
                MaxInstances = 5,
            };
            Item.autoReuse = true;
			Item.shoot = ProjectileID.Bullet;
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 16f;
			Item.noMelee = true;
			Item.crit = 14;
            Item.consumeAmmoOnLastShotOnly = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(1f, 1f);
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			
			recipe.AddIngredient(ItemID.ClockworkAssaultRifle, 1);
			recipe.AddIngredient(ItemID.TitaniumBar, 15);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();

            recipe2.AddIngredient(ItemID.ClockworkAssaultRifle, 1);
            recipe2.AddIngredient(ItemID.AdamantiteBar, 15);
            recipe2.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.Register();
        }


	}
}