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
    public class NatureBeam : ModProjectile
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
            Projectile.damage = 40;
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
            Projectile.knockBack = 16f;
        }
        public override void AI()
        {
            Projectile.ai[0] += 1f;
            FadeInAndOut();
            Projectile.velocity *= 0.99f;
            Projectile.scale *= 1.005f;
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
                Projectile.alpha -= 10;
                // Cap alpha before timer reaches 50 ticks
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;

                return;
            }

            // Fade out
            Projectile.alpha += 20;
            // Cal alpha to the maximum 255(complete transparent)
            if (Projectile.alpha > 255)
            {
                Projectile.alpha = 255;
                Projectile.Kill();
            }
               
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 300);
        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/NatureBeamef").Value;
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
                    lightColor.A = 0;
                    if (Projectile.alpha <= 0)
                        Main.EntitySpriteDraw(texture2, lerpedPos, null, lightColor * 0.1f * (1 - ((float)k / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                }
            }


            return true;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.GreenTorch, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f, Scale: 1.6f);
                Main.dust[dust].velocity *= 4f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item73, Projectile.position);
        }


    } 
}