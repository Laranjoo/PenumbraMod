using PenumbraMod.Content;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Buffs;
using System;

namespace PenumbraMod.Content.Items
{
	public class ElementaryWoodSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			 // DisplayName.SetDefault("Elementary Wood Sword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("An old elementary sword"
			+ "\nFires an [c/00ffff:Enchanted Shot]"
				+ "\n[c/00ff00:By right clicking], the blade will fire 3 Differents Elemental Shots! [c/cf8484:(With a 15 sec cooldown)]"
				+ "\n[c/cc00ff:Corruption]: Shoots a Eater of souls like projectile that slowly homes into enemies, dealing normal damage"
				+ "\n[c/1e90ff:Ice]: Shoots a ice crystal that deals high damage"
				+ "\n[c/adff2f:Jungle]: Shoots a Snacther like Projectile that quickly homes into enemies, dealing lower damage"); */

        }

		public override void SetDefaults()
		{
			Item.damage = 33;
			Item.DamageType = DamageClass.Melee;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 36;
			Item.useAnimation = 28;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 1000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<EMPTY>();
			Item.shootSpeed = 10f;
			Item.crit = 9;
		    
			
		}
		public override bool AltFunctionUse(Player player)
		{
			if (!player.HasBuff(ModContent.BuffType<ElementalCooldown>()))
				return true;
			return false;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            if (player.altFunctionUse == 2)
			{
				player.AddBuff(ModContent.BuffType<ElementalCooldown>(), 960);
				
				const int NumProjectiles = 1;

				for (int i = 0; i < NumProjectiles; i++)
				{
                    Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(20));
                    Vector2 newVelocity2 = velocity.RotatedBy(MathHelper.ToRadians(-20));
                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 1f - Main.rand.NextFloat(0.4f);
                    newVelocity2 *= 1f - Main.rand.NextFloat(0.4f);

                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<ProjectileSnacther>(), 23, knockback, player.whoAmI);
					Projectile.NewProjectileDirect(source, position, player.DirectionTo(Main.MouseWorld) * 12f, ModContent.ProjectileType<ProjectileIce>(), 60, knockback, player.whoAmI);
                    Projectile.NewProjectileDirect(source, position, newVelocity2, ModContent.ProjectileType<ProjectileCorruption>(), 40, knockback, player.whoAmI);

                }

				return true;
			}
			else
			{
                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<EnchantedShot>(), damage, knockback, player.whoAmI);
            }
			return true;
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 43f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }



        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			
			recipe.AddIngredient(ItemID.WoodenSword, 1);
			recipe.AddIngredient(ItemID.BorealWoodSword, 1);
			recipe.AddIngredient(ItemID.RichMahoganySword, 1);
			recipe.AddIngredient(ItemID.PalmWoodSword, 1);
			recipe.AddIngredient(ItemID.ShadewoodSword, 1);
			recipe.AddIngredient(ItemID.EnchantedSword, 1);
			recipe.AddIngredient(ModContent.ItemType<RustyCopperSword>(), 1);
			recipe.AddIngredient(ItemID.Diamond, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

            Recipe recipe2 = CreateRecipe();

            recipe2.AddIngredient(ItemID.WoodenSword, 1);
            recipe2.AddIngredient(ItemID.BorealWoodSword, 1);
            recipe2.AddIngredient(ItemID.RichMahoganySword, 1);
            recipe2.AddIngredient(ItemID.PalmWoodSword, 1);
            recipe2.AddIngredient(ItemID.EbonwoodSword, 1);
            recipe2.AddIngredient(ItemID.EnchantedSword, 1);
            recipe2.AddIngredient(ModContent.ItemType<RustyCopperSword>(), 1);
            recipe2.AddIngredient(ItemID.Diamond, 5);
            recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}