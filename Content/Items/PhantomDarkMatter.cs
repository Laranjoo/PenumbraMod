using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;


namespace PenumbraMod.Content.Items
{
    public class PhantomDarkMatter : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Phantom DarkMatter"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            Main.projFrames[Projectile.type] = 7;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 400;
            Projectile.width = 40;
            Projectile.height = 30;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;
            Projectile.light = 0.75f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DarkMatter>(), 180);
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
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/PhantomDarkMatterEf").Value;
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
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) *((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, sourceRectangle, color, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects, 0);
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
            Color result = Color.Lerp(Color.Lerp(new Color(17, 0, 36), new Color(9, 0, 19), transitToDark * 0.5f), Color.Black, lerpValue) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            return result;
        }

        private float StripWidth(float progressOnStrip)
        {
            float lerpValue = Utils.GetLerpValue(0f, 0.06f + transitToDark * 0.01f, progressOnStrip, clamped: true);
            lerpValue = 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(30f + transitToDark * 16f, 8f, Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true)) * lerpValue;
        }
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 29; k++)
            {
                int dust =  Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<DarkMatter2>(), Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f, Scale: 1.4f);
                Main.dust[dust].velocity *= 6.0f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item109, Projectile.position);
        }
        public override void AI()
        {
            Projectile.ai[0] += 1f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            int dust = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<DarkMatter2>(), 0f, 0f, 0);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 3f;
            Main.dust[dust].scale = (float) Main.rand.Next(100, 170) * 0.012f;

            Lighting.AddLight(Projectile.Center, Color.Black.ToVector3() * 0.78f);
            float maxDetectRadius = 300f; // The maximum radius at which a projectile can detect a target
            float projSpeed = 16f;
            NPC closestNPC = FindClosestNPC(maxDetectRadius);
            if (closestNPC == null)
                return;

            // If found, change the velocity of the projectile and turn it in the direction of the target
            // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
            Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
        }
        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                // Check if NPC able to be targeted. It means that NPC is
                // 1. active (alive)
                // 2. chaseable (e.g. not a cultist archer)
                // 3. max life bigger than 5 (e.g. not a critter)
                // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                // 5. hostile (!friendly)
                // 6. not immortal (e.g. not a target dummy)
                if (target.CanBeChasedBy())
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }
    } 
}