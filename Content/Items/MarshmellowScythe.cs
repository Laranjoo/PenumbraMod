using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Dusts;
using PenumbraMod.Content.Items.Consumables;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class MarshmellowScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Marshmellow Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("Fluffy enough for mass mob killing!" +
                "\nSpins the scythe through player" +
				"\n[c/d1d8d9:Special ability:] When used, The scythe fires 2 marshmellow balls that sticks into the ground"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 18;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 50;
			Item.height = 38;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 3050;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 1f;
            Item.noUseGraphic = false;
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
            return true;
        }
       
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                const int NumProjectiles = 2;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(30));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 8f - Main.rand.NextFloat(0f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<MarshBall>(), damage, knockback, player.whoAmI);
                }
            }
                        
           
            return false; // return false to stop vanilla from calling Projectile.NewProjectile.
        }

        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<Marshmellow>(), 20)
			.AddIngredient(ItemID.Wood, 15)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
        public class MarshBall : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Marshmellow Ball");
               
            }

            public override void SetDefaults()
            {
                Projectile.damage = 12;
                Projectile.width = 18;
                Projectile.height = 18;
                Projectile.aiStyle = 68;
                // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
                Projectile.friendly = true;
                Projectile.hostile = false;
                //projectile.magic = true;
                //projectile.extraUpdates = 100;
                Projectile.timeLeft = 360; // lowered from 300
                Projectile.penetrate = 1;
                Projectile.tileCollide = true;
                Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            }
               
            public override bool OnTileCollide(Vector2 oldVelocity)
            {
                Vector2 vel = Vector2.Zero;
                Projectile.velocity = Vector2.Zero;
                Projectile.rotation += 0f;
                Projectile.Kill();
                return false;
            }
            public override void Kill(int timeLeft)
            {
                Vector2 launchVelocity = new Vector2(0, 1); // Create a velocity moving the left.
                for (int i = 0; i < 1; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<MarshGround>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner, Projectile.scale *= 1.1f);
                }
                SoundEngine.PlaySound(SoundID.Item85, Projectile.position);
            }

        }
        public class MarshGround : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Marshmellow Blob");
                Main.projFrames[Projectile.type] = 6;
            }
            public override void SetDefaults()
            {
                Projectile.damage = 10;
                Projectile.width = 24;
                Projectile.height = 10;
                Projectile.aiStyle = 0;
                // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
                Projectile.friendly = true;
                Projectile.hostile = false;
                //projectile.magic = true;
                //projectile.extraUpdates = 100;
                Projectile.timeLeft = 180; // lowered from 300
                Projectile.penetrate = -1;
                Projectile.tileCollide = true;
            }
            public override void AI()
            {
                if (Projectile.velocity.Y != 0)
                {
                    Projectile.rotation += 0.3f;
                    if (++Projectile.frameCounter >= 3)
                    {
                        Projectile.frameCounter = 0;
                        Projectile.frame = 5;
                        if (++Projectile.frame >= 6)
                            Projectile.frame = 5;
                    }
                }
                else
                {
                    if (++Projectile.frameCounter >= 8)
                    {
                        Projectile.frameCounter = 0;
                        if (++Projectile.frame >= 4)
                            Projectile.frame = 0;
                    }
                    Projectile.rotation = 0;
                }
                
                if (Main.rand.NextBool(14))
                {
                    int dust = Dust.NewDust(Projectile.position, 24, 10, ModContent.DustType<MarshmellowDust>(), 2f, 0f, 0);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.008f;
                }
               
            }
            public override bool OnTileCollide(Vector2 oldVelocity)
            {
                Projectile.velocity = Vector2.Zero;
                return false;
            }
            public override void Kill(int timeLeft)
            {
                SoundEngine.PlaySound(SoundID.Item111, Projectile.position);
            }
            
        }
    }
}