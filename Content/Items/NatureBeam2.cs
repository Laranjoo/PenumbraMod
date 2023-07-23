using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;


namespace PenumbraMod.Content.Items
{
    public class NatureBeam2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nature Beam"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 80;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.knockBack = 8f;
        }
        public override void AI()
        {
            Projectile.ai[0] += 1f;
           
            FadeInAndOut();
            Projectile.scale *= 1.009f;
            Projectile.velocity *= 0.99f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
           
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 1.2f);
        }
        public void FadeInAndOut()
        {
            // If last less than 50 ticks — fade in, than more — fade out
            if (Projectile.ai[0] <= 40f)
            {
                // Fade in
                Projectile.alpha -= 20;
                // Cap alpha before timer reaches 50 ticks
                if (Projectile.alpha < 190)
                    Projectile.alpha = 190;

                return;
            }

            // Fade out
            Projectile.alpha += 20;
            // Cal alpha to the maximum 255(complete transparent)
            if (Projectile.alpha > 255)
                Projectile.alpha = 255;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 300);
        }
        public override bool PreDrawExtras()
        {
            Projectile.type = ModContent.ProjectileType<NatureBeam2>();
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
           Projectile.type = ModContent.ProjectileType<NatureBeam2>();


            if (Projectile.ai[0] == 1f)
            {
                Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;
                Vector2 drawPosition = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                Vector2 drawOrigin = new Vector2(projectileTexture.Width, projectileTexture.Height) / 2f;
                Color drawColor = Projectile.GetAlpha(lightColor);
                drawColor.A = 127;
                drawColor *= 0.5f;

                SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                for (float transparancy = 1f; transparancy >= 0f; transparancy -= 0.125f)
                {
                    float opacity = 1f - transparancy;
                    Vector2 drawAdjustment = Projectile.velocity * transparancy;
                    Main.EntitySpriteDraw(projectileTexture, drawPosition + drawAdjustment, null, drawColor * opacity, Projectile.rotation, drawOrigin, Projectile.scale * 1.15f * MathHelper.Lerp(0.9f, 1.2f, opacity), spriteEffects, 0);
                }
            }

            return base.PreDraw(ref lightColor);
        }
       

    } 
}