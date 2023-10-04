using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.DamageClasses;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;


namespace PenumbraMod.Content.Items
{
    public class IceSickle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ice Sickle"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 50;
            Projectile.width = 52;
            Projectile.height = 44;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 80;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 300);
        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/IceSickleef").Value;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
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
                    Main.EntitySpriteDraw(texture, lerpedPos, null, color * Projectile.Opacity, lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                }
            }


            return true;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(102, 255, 255, 0) * Projectile.Opacity;
        }
        public int timer;
        public override void AI()
        {
            timer++;
            Lighting.AddLight(Projectile.Center, Color.LightBlue.ToVector3() * 0.93f);
            int dust = Dust.NewDust(Projectile.Center, 2, 1, DustID.IceRod, 0f, 0f, 0);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 5.0f;
            Main.dust[dust].scale = (float) Main.rand.Next(100, 150) * 0.008f;
            Projectile.velocity *= 0.95f;
            Projectile.rotation += 0.4f;
            if (timer > 30)
            {
                Projectile.rotation -= 0.1f;
                Projectile.alpha += 5;
            }
            if (timer > 50)
            {
                Projectile.rotation -= 0.2f;
            }
            if (timer > 60)
            {
                Main.dust[dust].scale = (float)Main.rand.Next(100, 150) * 0.004f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 30; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.IceRod, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1f);
                Main.dust[dust].velocity *= 6.0f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
        }
    } 
}