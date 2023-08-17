using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Armors
{
    public class Eyeglow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Eye glow");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.width = 9;
            Projectile.height = 9;
            Projectile.timeLeft = 9999;
            Projectile.penetrate = -1;
            Projectile.light = 1f;
            Projectile.hide = false;
            Projectile.alpha = 0;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
           
            // Redraw the projectile with the color not influenced by light
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
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
                    Main.EntitySpriteDraw(texture, lerpedPos, null, color * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                }
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(230, 87, 120, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            projOwner.heldProj = Projectile.whoAmI;
            Projectile.rotation += 0.2f;
            Vector2 playerCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter);
            Projectile.Center = playerCenter + new Vector2(6, 11);
            Projectile.timeLeft = 2;

            if (projOwner.dead && !projOwner.active && !Main.dayTime)
            {//Disappear when player dies
                
                Projectile.Kill();
            }
            if (!Main.dayTime)
            {
                Projectile.timeLeft = 0;
                Projectile.Kill();
            }

            if (!(projOwner.armor[0].type == ModContent.ItemType<PrismHelm>()))
            {
                Projectile.timeLeft = 0;
                Projectile.Kill();
            }
            if (!(projOwner.armor[1].type == ModContent.ItemType<PrismArmor>()))
            {
                Projectile.timeLeft = 0;
                Projectile.Kill();
            }
            if (!(projOwner.armor[2].type == ModContent.ItemType<PrismLeggings>()))
            {
                Projectile.timeLeft = 0;
                Projectile.Kill();
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 30; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PurpleTorch, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f, Scale: 0.4f);
                Main.dust[dust].velocity *= 2.0f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item70, Projectile.position);
        }
    }
}