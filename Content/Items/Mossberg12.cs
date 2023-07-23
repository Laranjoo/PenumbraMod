using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content;
using Terraria.Audio;
using Terraria.DataStructures;

namespace PenumbraMod.Content.Items
{
	public class Mossberg12 : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Mossberg-12"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("Fires a lot of bullets dealing a lot of damage"
				+ "\n'This is for those who can't aim...'"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 66;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 122;
			Item.height = 18;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 1000;
			Item.rare = ItemRarityID.Yellow;
            Item.UseSound = new SoundStyle("PenumbraMod/Assets/Sounds/Items/HeavyShotgun1")
            {
                Volume = 1.2f,
                PitchVariance = 0.4f,
                MaxInstances = 3,
            };
            Item.autoReuse = true;
			Item.shoot = ProjectileID.Bullet;
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 16f;
			Item.noMelee = true;
            Item.crit = 12;
		}

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8f, 1f);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 86f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.velocity = player.DirectionTo(Main.MouseWorld) * -2f;
            const int NumProjectiles = 10;
            for (int i = 0; i < NumProjectiles; i++)
            {        
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(20));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(0.4f);

                // Create a projectile.
                Projectile.NewProjectileDirect(source, position, newVelocity, type, Item.damage, knockback, player.whoAmI);
            }
            return true;

        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.QuadBarrelShotgun, 1);
            recipe.AddIngredient(ModContent.ItemType<MeltedEmber>(), 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }



    }
}