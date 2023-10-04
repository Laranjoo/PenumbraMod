using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class TheMagebladeSwing : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Mageblade"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 100;
            Projectile.width = 104;
            Projectile.height = 104;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueTorch, 0f, 0f, 0);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 5.0f;
            Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.010f;

            Projectile.velocity = (Main.MouseWorld - Projectile.Center) / 8;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (!player.channel || player.CCed)
            {
                Projectile.Kill();
                Projectile.timeLeft = 0;
                return;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 60)
            {
                Vector2 launchVelocity = new Vector2(-5, 1); // Create a velocity moving the left.
                for (int i = 0; i < 1; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedByRandom(360);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<TheMagebladeExplosion>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                }
            }
            if (Projectile.ai[0] == 90)
            {
                Projectile.ai[0] = 20;
            }
            Projectile.timeLeft = 2;
            Lighting.AddLight(Projectile.Center, Color.LightBlue.ToVector3() * 0.93f);
            Projectile.rotation += 0.4f;
        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/TheMagebladeSwingEf").Value;
            // Redraw the projectile with the color not influenced by light
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    return false;
                for (float j = 0; j < 1; j += 0.0625f)
                {
                    Vector2 lerpedPos;
                    if (i > 0)
                        lerpedPos = Vector2.Lerp(Projectile.oldPos[i - 1], Projectile.oldPos[i], easeInOutQuad(j));
                    else
                        lerpedPos = Vector2.Lerp(Projectile.position, Projectile.oldPos[i], easeInOutQuad(j));
                    float lerpedAngle;
                    if (i > 0)
                        lerpedAngle = Utils.AngleLerp(Projectile.oldRot[i - 1], Projectile.oldRot[i], easeInOutQuad(j));
                    else
                        lerpedAngle = Utils.AngleLerp(Projectile.rotation, Projectile.oldRot[i], easeInOutQuad(j));
                    lerpedPos += Projectile.Size / 2;
                    lerpedPos -= Main.screenPosition;
                    Main.EntitySpriteDraw(texture, lerpedPos, null, Color.White * 0.2f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 0);
                }
            }
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 50; k++)
            {
                int dust =  Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueTorch, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 2.9f);
                Main.dust[dust].velocity *= 6.0f;
            }

            Vector2 launchVelocity = new Vector2(-5, 1); // Create a velocity moving the left.
            for (int i = 0; i < 10; i++)
            {
                // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<TheMagebladeExplosion>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);

            }
          
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item109, Projectile.position);
        }
    }
}
