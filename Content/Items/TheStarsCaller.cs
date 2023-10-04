using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
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
	public class TheStarsCaller : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("The Stars Caller"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("This scythe is from the sky!... literally!" +
                "\nThrows the scythe that penetrates enemies, but gets reduced damage every hit" +
                "\n[c/9f83d9:Special ability:] When used, the scythe spawns stars from the sky!"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 42;
			Item.height = 36;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = 1000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TheStarsProj>();
            Item.shootSpeed = 40;
            Item.noUseGraphic = true;
            
		}
        public override bool CanUseItem(Player player)
        {
           
			return player.ownedProjectileCounts[ModContent.ProjectileType<TheStarsProj>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                const int NumProjectiles = 1;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                    float ceilingLimit = target.Y;
                    if (ceilingLimit > player.Center.Y - 200f)
                    {
                        ceilingLimit = player.Center.Y - 200f;
                    }
                    // Loop these functions 3 times.
                    for (int e = 0; e < 3; e++)
                    {
                        position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
                        position.Y -= 100 * e;
                        Vector2 heading = target - position;

                        if (heading.Y < 0f)
                        {
                            heading.Y *= -1f;
                        }

                        if (heading.Y < 20f)
                        {
                            heading.Y = 20f;
                        }

                        heading.Normalize();
                        heading *= velocity.Length();
                        heading.Y += Main.rand.Next(-40, 41) * 0.02f;
                        Projectile.NewProjectile(source, position, heading, ProjectileID.HallowStar, damage * 2, knockback, player.whoAmI, 0f, ceilingLimit);
                    }

                }

            }

            return true;
        }
        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.FallenStar, 15)
            .AddIngredient(ItemID.Feather, 10)
            .AddIngredient(ItemID.PlatinumBar, 15)
            .AddIngredient(ItemID.SunplateBlock, 7)
			.AddTile(TileID.Anvils)
			.Register();
		}
        public class TheStarsProj : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("The Stars Caller Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
                ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            }

            public override void SetDefaults()
            {
                Projectile.damage = 24;
                Projectile.width = 62;
                Projectile.height = 56;
                Projectile.aiStyle = 0;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.penetrate = -1;
                Projectile.timeLeft = 360;
                Projectile.ignoreWater = false;
                Projectile.tileCollide = false;
                Projectile.ownerHitCheck = true;
                Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            }

            public override void AI()
            {
                Player player = Main.player[Projectile.owner];
                Projectile.velocity *= 0.95f;

                Projectile.ai[0] += 1f;
                float projSpeed = 40;
                if (Projectile.ai[0] >= 15f)
                {
                    Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
                    if (Projectile.Distance(player.Center) <= projSpeed)
                    {
                        Projectile.Kill();
                        return;
                    }

                }

                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.YellowStarDust, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                Main.dust[dust].velocity *= 2f;
                Projectile.rotation += 0.4f;

            }
            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                Projectile.damage -= 6;
            }
            public static float easeInOutQuad(float x)
            {
                return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
            }
            public override bool PreDraw(ref Color lightColor)
            {
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Texture2D texture2 = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/TheStarsProjEffect").Value;
                // Redraw the projectile with the color not influenced by light
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                    if (Projectile.oldPos[k] == Vector2.Zero)
                        return false;
                    for (float j = 0; j < 1; j += 0.0625f)
                    {
                        Vector2 lerpedPos;
                        if (k > 0)
                            lerpedPos = Vector2.Lerp(Projectile.oldPos[k - 1], Projectile.oldPos[k], easeInOutQuad(j));
                        else
                            lerpedPos = Vector2.Lerp(Projectile.position, Projectile.oldPos[k], easeInOutQuad(j));
                        float lerpedAngle;
                        if (k > 0)
                            lerpedAngle = Utils.AngleLerp(Projectile.oldRot[k - 1], Projectile.oldRot[k], easeInOutQuad(j));
                        else
                            lerpedAngle = Utils.AngleLerp(Projectile.rotation, Projectile.oldRot[k], easeInOutQuad(j));
                        lerpedPos += Projectile.Size / 2;
                        lerpedPos -= Main.screenPosition;
                        Main.EntitySpriteDraw(texture2, lerpedPos, null, Color.White * 0.3f * (1 - ((float)k / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                    }
                }

                
                return true;
            }
            public override void OnKill(int timeLeft)
            {
                for (int k = 0; k < 10; k++)
                {
                    int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Harpy, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                    Main.dust[dust].velocity *= 1f;
                }
                // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item18, Projectile.position);
            }


        }
    }
}