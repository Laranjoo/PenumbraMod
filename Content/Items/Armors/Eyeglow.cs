using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using PenumbraMod.Effects;
using Terraria.Graphics.Shaders;
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
        private static VertexStrip _vertexStrip = new VertexStrip();
        private float transitToDark;
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width * 0.5f, Projectile.height * 0.5f);
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
                    float size = Projectile.scale * (Projectile.oldPos.Length - i) / (Projectile.oldPos.Length * 1.05f);
                    Main.EntitySpriteDraw(texture, lerpedPos, null, color * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), size, SpriteEffects.None, 0);
                }
            }
            transitToDark = Utils.GetLerpValue(0f, 6f, Projectile.localAI[0], clamped: true);
            MiscShaderData miscShaderData = GameShaders.Misc["FlameLash"];
            miscShaderData.UseSaturation(-2f);
            miscShaderData.UseOpacity(MathHelper.Lerp(4f, 8f, transitToDark));
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2f, includeBacksides: true);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            return true;
        }
        private Color StripColors(float progressOnStrip)
        {
            float lerpValue = Utils.GetLerpValue(0f - 0.1f * transitToDark, 0.7f - 0.2f * transitToDark, progressOnStrip, clamped: true);
            Color result = Color.Lerp(Color.Lerp(new Color(230, 87, 120, 0), new Color(230, 87, 120, 0), transitToDark * 0.5f), new Color(230, 87, 120, 0), lerpValue) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            return result;
        }

        private float StripWidth(float progressOnStrip)
        {
            float lerpValue = Utils.GetLerpValue(0f, 0.06f + transitToDark * 0.01f, progressOnStrip, clamped: true);
            lerpValue = 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(7f + transitToDark * 16f, 8f, Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true)) * lerpValue;
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