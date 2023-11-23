﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class CorrosivePickaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Melee;
            Item.width = 42;
            Item.height = 42;
            Item.useTime = 5;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 8;
            Item.value = 30000;
            Item.rare = ItemRarityID.Lime;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CorrosivePickaxeS>();
            Item.shootSpeed = 2f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
            Item.pick = 205;
            Item.tileBoost += 2;
        }
        bool notboollol = true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[type] < 1)
            {
                int basic = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (notboollol == true)
                {
                    Main.projectile[basic].ai[1] = -1;
                    notboollol = false;
                }
                else
                {
                    Main.projectile[basic].ai[1] = 1;
                    notboollol = true;
                }
                Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
            }

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CorrosiveShard>(), 12)
                 .AddIngredient(ModContent.ItemType<CorrodedPlating>(), 10)

               .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class CorrosivePickaxeS : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            //Projectile.aiStyle = 1;
            // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
            Projectile.friendly = true;
            //projectile.magic = true;
            //projectile.extraUpdates = 100;
            Projectile.timeLeft = 10; // lowered from 300
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;
            Projectile.light = 1f;
        }
        Vector2 dir = Vector2.Zero;
        Vector2 hlende = Vector2.Zero;
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/CorrosivePickaxeSF").Value;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero)
                    return false;
                for (float j = 0.0625f; j < 1; j += 0.0625f)//not sure if it will work actually
                {
                    Vector2 oldPosForLerp = k > 0 ? Projectile.oldPos[k - 1] : Projectile.position;
                    Vector2 lerpedPos = Vector2.Lerp(oldPosForLerp, Projectile.oldPos[k], easeInOutQuad(j));
                    float oldRotForLerp = k > 0 ? Projectile.oldRot[k - 1] : Projectile.rotation;
                    float lerpedAngle = Utils.AngleLerp(oldRotForLerp, Projectile.oldRot[k], easeInOutQuad(j));
                    lerpedPos += Projectile.Size / 2;
                    lerpedPos -= Main.screenPosition;
                    Color finalColor = lightColor * 0.5f * (1 - ((float)k / (float)Projectile.oldPos.Length));
                    finalColor.A = 0;//acts like additive blending without spritebatch stuff
                    if (Projectile.ai[1] == -1)
                        Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                    if (Projectile.ai[1] == 1)
                        Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, texture.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
                }
            }         
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            if (Projectile.ai[1] == -1)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            if (Projectile.ai[1] == 1)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
            float scaleFactor = 60f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 23f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            // This Rectangle is the width and height of the Jousting Lance's hitbox which is used for the first step of collision.
            // You will need to modify the last two numbers if you have a bigger or smaller Jousting Lance.
            // Vanilla uses (0, 0, 300, 300) which that is quite large for the size of the Jousting Lance.
            // The size doesn't matter too much because this rectangle is only a basic check for the collision (the hit-line is much more important).
            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 300, 300);

            // Set the position of the large rectangle.
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;
            //Dust.NewDustPerfect(hitLineEnd, DustID.JungleTorch, Projectile.DirectionTo(Main.MouseWorld) * 2f, 0, default, Scale: 0.8f);
            hlende = hitLineEnd;
            // The following is for debugging the size of the hit line. This will allow you to easily see where it starts and ends. 
            // First check that our large rectangle intersects with the target hitbox.
            // Then we check to see if a line from the tip of the Jousting Lance to the "end" of the lance intersects with the target hitbox.
            if (/*lanceHitboxBounds.Intersects(targetHitbox)
                && */Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
            {
                return true;
            }
            return false;
        }
        public override void AI()
        {
            if (dir == Vector2.Zero)
            {
                dir = Main.MouseWorld;
                Projectile.rotation = (MathHelper.PiOver2 * Projectile.ai[1]) - MathHelper.PiOver4 + Projectile.DirectionTo(Main.MouseWorld).ToRotation();
            }
            Player player = Main.player[Projectile.owner];
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 90 + 0.78f);
            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.ai[0] += 1f;
            Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((20 - Projectile.ai[0])));
            if (Projectile.ai[0] == 1)
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 0.5f);
        }
    }
}

