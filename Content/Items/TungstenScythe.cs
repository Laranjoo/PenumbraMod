using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Dusts;
using PenumbraMod.Content.Items.Consumables;
using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class TungstenScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tungsten Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("You may like it" +
                "\n[c/38594c:Special ability:] When used, it spawns tungsten spikes on the ground"); */

        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 52;
            Item.height = 42;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 5920;
            Item.rare = 0;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 0f;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                Item.useStyle = ItemUseStyleID.Swing;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Swing;
            }
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[ModContent.ProjectileType<TungstenSpike>()] < 5;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                const int NumProjectiles = 1;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    SoundEngine.PlaySound(SoundID.Item71, player.position);
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(50));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 0f - Main.rand.NextFloat(0f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<TungstenSpike>(), damage, knockback, player.whoAmI);

                }

            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.TinBar, 15)
            .AddIngredient(ItemID.Wood, 7)
            .AddTile(TileID.Anvils)
            .Register();
        }

        public class TungstenSpike : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Tungsten Spike"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.

            }

            public override void SetDefaults()
            {
                Projectile.damage = 15;
                Projectile.width = 42;
                Projectile.height = 50;
                Projectile.aiStyle = 756;
                AIType = ProjectileID.SharpTears;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.penetrate = -1;
                Projectile.timeLeft = 120;
                Projectile.ignoreWater = false;
                Projectile.tileCollide = false;
            }


            public override void AI()
            {
                int y0 = 1;
                bool foundSurface = false;
                while (y0 > Main.MouseWorld.Y && y0 < Main.MouseWorld.Y + 1000 && foundSurface == false)
                {
                    if (WorldGen.SolidTile((int)Projectile.Center.X, y0))
                    {
                        foundSurface = true;
                        break;
                    }
                    y0++;
                }
            }

            public override void Kill(int timeLeft)
            {
                // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
        }

    }
}