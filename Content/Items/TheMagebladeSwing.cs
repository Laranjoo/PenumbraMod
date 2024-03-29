﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Items.PrismaticScythe;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class TheMagebladeSwing : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 208;
            Projectile.height = 208;
            Projectile.friendly = true;
            Projectile.timeLeft = 28;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
        }
        Vector2 dir = Vector2.Zero;
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
            float scaleFactor = 200f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 30f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            // This Rectangle is the width and height of the Jousting Lance's hitbox which is used for the first step of collision.
            // You will need to modify the last two numbers if you have a bigger or smaller Jousting Lance.
            // Vanilla uses (0, 0, 300, 300) which that is quite large for the size of the Jousting Lance.
            // The size doesn't matter too much because this rectangle is only a basic check for the collision (the hit-line is much more important).
            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 208, 208);

            // Set the position of the large rectangle.
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;

            hlende = hitLineEnd;
            // First check that our large rectangle intersects with the target hitbox.
            // Then we check to see if a line from the tip of the Jousting Lance to the "end" of the lance intersects with the target hitbox.
            if (/*lanceHitboxBounds.Intersects(targetHitbox)
                && */Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier, ref collisionPoint))
            {
                return true;
            }
            return false;
        }
        int a;
        public override void AI()
        {
            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.ai[2]++;
            a++;
            Player player = Main.player[Projectile.owner];
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 90);
            if (dir == Vector2.Zero)
            {
                dir = Main.MouseWorld;
                Projectile.rotation = (MathHelper.PiOver2 * Projectile.ai[1]) - MathHelper.PiOver4 + Projectile.DirectionTo(Main.MouseWorld).ToRotation();
            }
            //FadeInAndOut();
            if (player.direction == 1)
                Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((11 - Projectile.ai[0])));
            if (player.direction == -1)
                Projectile.rotation -= (Projectile.ai[1] * MathHelper.ToRadians((11 - Projectile.ai[0])));

            if (Projectile.ai[2] <= 8)
            {
                Projectile.ai[0] += 0.01f;
                Projectile.ai[1] += 0.01f;
                Projectile.friendly = false;
            }     
            if (Projectile.ai[2] >= 9 && Projectile.ai[2] <= 16)
            {
                Projectile.ai[0] += 1f;
                Projectile.ai[1] += 1f;
                Projectile.scale += 0.07f;
                Projectile.friendly = true;
            }
            if (Projectile.ai[2] >= 17 && Projectile.ai[2] <= 22)
            {
                Projectile.ai[0] += 0.3f;
                Projectile.ai[1] += 0.3f;
            }
            if (Projectile.ai[2] >= 23 && Projectile.ai[2] <= 28)
            {
                Projectile.ai[0] += 0.01f;
                Projectile.ai[1] += 0.01f;
                Projectile.friendly = false;
            }
            if (Projectile.ai[2] >= 18 && Projectile.ai[2] <= 21)
                Projectile.scale -= 0.07f;
            if (a == 13)
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
            }
            
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.5f);

        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/TheMagebladeSwingEf").Value;
            // Redraw the projectile with the color not influenced by light
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
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
                    Color c = lightColor;
                    c.A = 0;
                    Main.EntitySpriteDraw(texture, lerpedPos, null, c * 0.2f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                }
            }
            Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
