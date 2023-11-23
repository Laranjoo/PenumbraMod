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
    public class ShadowFlameProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        { 
            Projectile.width = 47;
            Projectile.height = 45;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 5;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 60);
        }
        float t = 0f;
        float f = 0.09f;
        const float r = 0.02f;     
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/ShadowFlameProj").Value;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;
            Rectangle sourceRectangle = new(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float offsetX = 20f;
            origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;

            Vector2 drawOrigin = new(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero)
                    return false;
                for (float j = 0; j < 1; j += 0.0625f)
                {
                    float lerpedAngle;
                    if (k > 0)
                        lerpedAngle = Utils.AngleLerp(Projectile.oldRot[k - 1], Projectile.oldRot[k], easeInOutQuad(j));
                    else
                        lerpedAngle = Utils.AngleLerp(Projectile.rotation, Projectile.oldRot[k], easeInOutQuad(j));
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, sourceRectangle, color * t, lerpedAngle, drawOrigin, f, spriteEffects, 0);
                }    
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.Inflate(47/2, 45/2);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {

            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void AI()
        {
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(Projectile.position, 46, 44, DustID.CursedTorch, Projectile.velocity.X, 0f, 0);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 3f;
                Main.dust[dust].scale = (float)Main.rand.Next(100, 170) * 0.012f;
            }
            if (++Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            Projectile.rotation += 0.3f;
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 13)
            {
                Projectile.alpha -= 30;
                t += r;
                if (t > 0.7f)
                    t = 0.7f;
                f += 0.1f;
                if (f > 2.33f)
                    f = 2.33f;
            }
            else
            {
                Projectile.alpha += 20;
                t -= r;
                if (t < 0f)
                    t = 0f;
                f += 0.1f;
                if (f > 2.33f)
                    f = 2.33f;
                if (Projectile.alpha > 255)
                {
                    Projectile.Kill();
                }
            }
        }
    }
}
