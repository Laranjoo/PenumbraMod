using PenumbraMod.Content;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Buffs;
using System;
using PenumbraMod.Content.Items.Placeable;
using PenumbraMod.Content.Dusts;

namespace PenumbraMod.Content.Items
{
	public class RozeQuartzStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.crit = 3; 
			Item.mana = 21;
			Item.DamageType = DamageClass.Magic;
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4;
			Item.value = 12000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<ConfettiProj>();
			Item.shootSpeed = 10f;
			Item.noMelee = true;			
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const int NumProjectiles = 15;
            for (int i = 0; i < NumProjectiles; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(20));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(0.4f);

                // Create a projectile.
                Projectile.NewProjectileDirect(source, position, newVelocity, type, Item.damage, knockback, player.whoAmI);
				Dust.NewDust(position, 0, 0, DustID.Confetti_Blue, newVelocity.X, newVelocity.Y);
                Dust.NewDust(position, 0, 0, DustID.Confetti_Green, newVelocity.X, newVelocity.Y);
                Dust.NewDust(position, 0, 0, DustID.Confetti_Pink, newVelocity.X, newVelocity.Y);
                Dust.NewDust(position, 0, 0, DustID.Confetti_Yellow, newVelocity.X, newVelocity.Y);
            }
            return true;

        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 44f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Confetti, 15);
			recipe.AddIngredient(ItemID.BubbleWand, 6);
            recipe.AddIngredient(ModContent.ItemType<RozeQuartz>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
	class ConfettiProj : ModProjectile
	{
        public override string Texture => "PenumbraMod/EMPTY";
        public override void SetDefaults()
        {
            Projectile.damage = 12;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 0;
            Projectile.alpha = 255;
            Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.timeLeft = 20;
        }      
    }
}