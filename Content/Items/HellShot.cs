using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Effects;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;


namespace PenumbraMod.Content.Items
{
    public class HellShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hell Shot"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            Main.projFrames[Projectile.type] = 9;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 50;
            Projectile.width = 26;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 280;
            Projectile.light = 0.75f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 120);
        }
        private static VertexStrip _vertexStrip = new VertexStrip();
        private float transitToDark;
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
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
            Color result = Color.Lerp(Color.Lerp(Color.White, Color.Orange, transitToDark * 0.5f), Color.Red, lerpValue) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A /= 8;
            return result;
        }

        private float StripWidth(float progressOnStrip)
        {
            float lerpValue = Utils.GetLerpValue(0f, 0.06f + transitToDark * 0.01f, progressOnStrip, clamped: true);
            lerpValue = 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(12f + transitToDark * 16f, 8f, Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true)) * lerpValue;
        }
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 30; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LavaMoss, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f, Scale: 1.4f);
                Main.dust[dust].velocity *= 4f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
            Projectile.direction = 1;

            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.LavaMoss, 1f, 0f, 0, Color.Yellow, 1);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 3f;
            Main.dust[dust].scale = (float) Main.rand.Next(80, 140) * 0.008f;

            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() * 0.78f);
        }
        
    } 
}