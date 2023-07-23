using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class FPJ : ModProjectile
	{
       
		public override void SetStaticDefaults()
		{
		 // DisplayName.SetDefault("Magical Foil"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line. 
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }
        Texture2D projtexture;
        Texture2D itemtexture;
        public override void Unload()
        {
            projtexture?.Dispose();
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
            Projectile.scale = 2f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 0.4f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(255, 204, 239, 70) * Projectile.Opacity;
        }
       

        public override bool ShouldUpdatePosition() => false;
        public override bool? CanCutTiles() => false;
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.LightPink.ToVector3() * 0.80f);
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
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            return;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.position, Projectile.position + Projectile.rotation.ToRotationVector2() * 304, 14, ref _);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            projtexture ??= ModContent.Request<Texture2D>(Texture, AssetRequestMode.ImmediateLoad).Value;
            itemtexture ??= ModContent.Request<Texture2D>(ModContent.GetInstance<MagicalFoilProjectile>().Texture, AssetRequestMode.ImmediateLoad).Value;
            DrawProj(Projectile);
            return false;
        }
        
        private void DrawProj(Projectile proj)
        {
            int num = 3;
            int num2 = 2;
            Vector2 value = proj.Center - proj.rotation.ToRotationVector2() * num2;
            float num3 = Main.rand.NextFloat();
            float scale = Utils.GetLerpValue(0f, 0.3f, num3, true) * Utils.GetLerpValue(1f, 0.5f, num3, true);
            Color color = Color.White * scale;
            Texture2D value2 = itemtexture;
            Vector2 origin = value2.Size() / 2f;
            float num4 = Main.rand.NextFloatDirection();
            float scaleFactor = 8f + MathHelper.Lerp(0f, 20f, num3) + Main.rand.NextFloat() * 6f;
            float num5 = proj.rotation + num4 * MathHelper.TwoPi * 0.04f;
            float num6 = num5 + MathHelper.PiOver4;
            Vector2 position = value + num5.ToRotationVector2() * scaleFactor + Main.rand.NextVector2Circular(8f, 8f) - Main.screenPosition;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (proj.rotation < -MathHelper.PiOver2 || proj.rotation > MathHelper.PiOver2)
            {
                num6 += MathHelper.PiOver2;
                spriteEffects |= SpriteEffects.FlipHorizontally;
            }
            Main.spriteBatch.Draw(value2, position, null, color, num6, origin, 2f, spriteEffects, 0f);
            for (int j = 0; j < num; j++)
            {
                float num7 = Main.rand.NextFloat();
                float num8 = Utils.GetLerpValue(0f, 0.3f, num7, true) * Utils.GetLerpValue(1f, 0.5f, num7, true);
                float amount = Utils.GetLerpValue(0f, 0.3f, num7, true) * Utils.GetLerpValue(1f, 0.5f, num7, true);
                float scaleFactor2 = MathHelper.Lerp(0.6f, 1f, amount);
                Color fairyQueenWeaponsColor = Color.White;
                Texture2D value3 = projtexture;
                Color color2 = fairyQueenWeaponsColor;
                color2 *= num8 * 0.5f;
                Vector2 origin2 = value3.Size() / 2f;
                Color value4 = Color.White * num8;
                value4.A /= 2;
                Color color3 = value4 * 0.5f;
                float num9 = 1f;
                float num10 = Main.rand.NextFloat() * 3.6f;
                float num11 = Main.rand.NextFloatDirection();
                Vector2 vector = new Vector2(3f + num10, 1f) * num9 * scaleFactor2;
                int num12 = 50;
                Vector2 value5 = proj.rotation.ToRotationVector2() * (j >= 1 ? 56 : 0);
                float num13 = 0.03f - j * 0.012f;
                float scaleFactor3 = 30f + MathHelper.Lerp(0f, num12, num7) + num10 * 16f;
                float num14 = proj.rotation + num11 * MathHelper.TwoPi * num13;
                float rotation = num14;
                Vector2 position2 = value + num14.ToRotationVector2() * scaleFactor3 + Main.rand.NextVector2Circular(20f, 20f) + value5 - Main.screenPosition;
                color2 *= num9;
                color3 *= num9;
                SpriteEffects effects = SpriteEffects.None;
                Main.spriteBatch.Draw(value3, position2, null, color2, rotation, origin2, vector, effects, 0f);
                Main.spriteBatch.Draw(value3, position2, null, color3, rotation, origin2, vector * 0.6f, effects, 0f);
            }
        }
        
    }
}