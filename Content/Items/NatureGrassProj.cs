using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class NatureGrassProj : ModProjectile
    {
        public override string Texture => "PenumbraMod/EMPTY";
        Texture2D itemtexture;
        public override void Unload()
        {
            itemtexture?.Dispose();
        }
        public override void SetDefaults()
        {
            Projectile.damage = 70;
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 4;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override bool ShouldUpdatePosition() => false;
        public override void AI()
        {
            Projectile.position = Main.player[Projectile.owner].MountedCenter;
            Projectile.rotation = (Main.MouseWorld - Main.player[Projectile.owner].MountedCenter).ToRotation() + Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) * 0.1f;
            Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
            if (!Main.mouseLeft)
            {
                Projectile.Kill();
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DisableCrit();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 0;
            return;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.position, Projectile.position + Projectile.rotation.ToRotationVector2() * 100, 14, ref _);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            itemtexture ??= ModContent.Request<Texture2D>(ModContent.GetInstance<NatureGrass>().Texture, AssetRequestMode.ImmediateLoad).Value;
            DrawProj(Projectile);
            return false;
        }

        private void DrawProj(Projectile proj)
        {
            int num2 = 4;
            Vector2 value = proj.Center - proj.rotation.ToRotationVector2() * num2;
            float num3 = Main.rand.NextFloat();
            Texture2D value2 = itemtexture;
            Vector2 origin = value2.Size() / 2f;
            float num4 = Main.rand.NextFloatDirection();
            float scaleFactor = 20f + MathHelper.Lerp(0f, 20f, num3) + Main.rand.NextFloat() * 24f;
            float num5 = proj.rotation + num4 * MathHelper.TwoPi * 0.04f;
            float num6 = num5 + MathHelper.PiOver4;
            Vector2 position = value + num5.ToRotationVector2() * scaleFactor + Main.rand.NextVector2Circular(8f, 8f) - Main.screenPosition;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (proj.rotation < -MathHelper.PiOver2 || proj.rotation > MathHelper.PiOver2)
            {
                num6 += MathHelper.PiOver2;
                spriteEffects |= SpriteEffects.FlipHorizontally;
            }
            Main.spriteBatch.Draw(value2, position, null, Color.White, num6, origin, 1.2f, spriteEffects, 0f);          
        }
    }
}