using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class CorrosiveScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 92;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 72;
            Item.height = 60;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = 1;
            Item.knockBack = 8;
            Item.value = 32000;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CorrosiveScytheSwing>();
            Item.shootSpeed = 2f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CorrosiveScytheHold>()] < 1)
            {//Equip animation.
                int index = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<CorrosiveScytheHold>(), 0, 0, player.whoAmI, 0f);
            }
            if (player.HasBuff(ModContent.BuffType<CorrosiveForce>()))
            {
                Item.damage = 110;
            }
            else
            {
                Item.damage = 82;
            }
        }
        bool notboollol = true;
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<CorrosiveScytheSwing>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!player.HasBuff(ModContent.BuffType<ReaperControl>()))
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

            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CorrosiveScytheSpecial>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CorrosiveShard>(), 15)
                 .AddIngredient(ModContent.ItemType<CorrodedPlating>(), 13)

               .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class CorrosiveScytheSwing : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 160;
            //Projectile.aiStyle = 1;
            // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
            Projectile.friendly = true;
            //projectile.magic = true;
            //projectile.extraUpdates = 100;
            Projectile.timeLeft = 20; // lowered from 300
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            Projectile.extraUpdates = 1;
            Projectile.netImportant = true;
        }
        Vector2 dir = Vector2.Zero;
        Vector2 hlende = Vector2.Zero;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(ModContent.BuffType<Corrosion>(), 120);
        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/CorrosiveScytheSwingef").Value;
            Texture2D proj = TextureAssets.Projectile[Type].Value;

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
                    Color finalColor = lightColor * 0.3f * (1 - ((float)k / (float)Projectile.oldPos.Length));
                    finalColor.A = 0;//acts like additive blending without spritebatch stuff
                    if (Projectile.ai[1] == -1)
                        Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle - 0.78f, texture.Size() / 2, 1, SpriteEffects.None, 0);
                    if (Projectile.ai[1] == 1)
                        Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle - 0.78f, texture.Size() / 2, 1, SpriteEffects.FlipHorizontally, 0);
                }
            }

            if (Projectile.ai[1] == -1)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            if (Projectile.ai[1] == 1)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);

            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
            float scaleFactor = 85f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
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
            if (Main.rand.NextBool(3))
                Dust.NewDust(hitLineEnd, 5, 5, DustID.JungleTorch, rotationFactor, rotationFactor, 0, default, 0.8f);
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
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 90);
            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.ai[0] += 1f;
            Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((17 - Projectile.ai[0])));
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 0.5f);
        }

    }
    public class CorrosiveScytheHold : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/CorrosiveScythe";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Composite Sword");
        }
        public override void SetDefaults()
        {
            AIType = 0;
            Projectile.width = 72;
            Projectile.height = 60;
            Projectile.penetrate = -1;
            Projectile.light = 0.3f;
            Projectile.hide = false;
            Projectile.alpha = 0;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        bool firstSpawn = true;
        double deg;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            return true;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];

            if (firstSpawn)
            {

                firstSpawn = false;
            }
            Projectile.timeLeft = 10;

            if (Projectile.spriteDirection == 1)
            {//Adjust when facing the other direction

                Projectile.ai[1] = 280;
            }
            else
            {
                Projectile.ai[1] = 280;
            }
            deg = Projectile.ai[1];
            double rad = deg * (Math.PI / 180);
            double dist = 10;

            Projectile.position.X = projOwner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = projOwner.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
            if (Projectile.spriteDirection == 1)
            {//Adjust when facing the other direction

                Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(270f);
            }
            else
            {

                Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(270f);
            }
            //Projectile.Center += projOwner.gfxOffY * Vector2.UnitY;//Prevent glitchy animation.

            Projectile.ai[0]++;

            if (projOwner.dead && !projOwner.active)
            {//Disappear when player dies
                Projectile.timeLeft = 0;
                Projectile.Kill();
                Projectile.alpha = 255;
            }
            if (projOwner.HeldItem?.type != ModContent.ItemType<CorrosiveScythe>())
            {
                Projectile.Kill();
            }
            if (projOwner.ownedProjectileCounts[ModContent.ProjectileType<CorrosiveScytheSwing>()] >= 1
                || projOwner.ownedProjectileCounts[ModContent.ProjectileType<CorrosiveScytheSpecial>()] >= 1 || projOwner.channel)
            {
                Projectile.alpha = 255;
            }
            else
            {
                Projectile.alpha = 0;
            }
            //Orient projectile
            Projectile.direction = projOwner.direction;
            Projectile.spriteDirection = projOwner.direction;
        }

    }
    public class CorrosiveScytheSpecial : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

        }
        public override void SetDefaults()
        {
            Projectile.damage = 92;
            Projectile.width = 142;
            Projectile.height = 200;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 999999;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
            Projectile.netImportant = true;
        }
        public override void AI()
        {
            Projectile.ai[0] += 1f;
            Projectile.ai[1] += 1f;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI; // We tell the player that the drill is the held projectile, so it will draw in their hand
            player.SetDummyItemTime(2); // Make sure the player's item time does not change while the projectile is out
            if (Projectile.ai[1] < 195)
            {
                Projectile.Center = player.Center;
                if (Projectile.ai[1] <= 15)
                    Projectile.rotation -= 0.1f;
                if (Projectile.ai[1] >= 30 && Projectile.ai[1] <= 35 || Projectile.ai[1] >= 181 && Projectile.ai[1] <= 195)
                {
                    Projectile.rotation += 0.1f;
                    Projectile.friendly = true;
                }
                if (Projectile.ai[1] >= 35 && Projectile.ai[1] <= 40 || Projectile.ai[1] >= 161 && Projectile.ai[1] <= 180)
                    Projectile.rotation += 0.2f;

                if (Projectile.ai[1] >= 41 && Projectile.ai[1] <= 160)
                {
                    Projectile.rotation += 0.3f;
                    if (Projectile.ai[0] == 32)
                        SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
                    if (Projectile.ai[0] >= 42)
                        Projectile.ai[0] = 30;
                }
                if (Projectile.ai[1] == 162)
                    Projectile.friendly = false;
            }
            else
                Projectile.Kill();
            Lighting.AddLight(Projectile.Center, Color.LightGreen.ToVector3() * 1.7f);


        }
        Vector2 dir = Vector2.Zero;
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
            float scaleFactor = 120f;
            float scaleFactor2 = -120f;// How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 24f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            // This Rectangle is the width and height of the Jousting Lance's hitbox which is used for the first step of collision.
            // You will need to modify the last two numbers if you have a bigger or smaller Jousting Lance.
            // Vanilla uses (0, 0, 300, 300) which that is quite large for the size of the Jousting Lance.
            // The size doesn't matter too much because this rectangle is only a basic check for the collision (the hit-line is much more important).
            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 284, 284);

            // Set the position of the large rectangle.
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;
            Vector2 hitLineEnd2 = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor2;
            if (Projectile.friendly)
            {
                if (Main.rand.NextBool(5))
                {
                    Dust.NewDust(hitLineEnd, 5, 5, DustID.JungleTorch, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, 0, default, 0.8f);
                    Dust.NewDust(hitLineEnd2, 5, 5, DustID.JungleTorch, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, 0, default, 0.8f);
                }
                hlende = hitLineEnd;
                // First check that our large rectangle intersects with the target hitbox.
                // Then we check to see if a line from the tip of the Jousting Lance to the "end" of the lance intersects with the target hitbox.
                if (/*lanceHitboxBounds.Intersects(targetHitbox)
                && */Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
                if (/*lanceHitboxBounds.Intersects(targetHitbox)
                && */Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd2, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
            }       
            return false;
        }

        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        float a = 0.2f;
        float b;
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/CorrosiveScytheSpecialF").Value;
            Texture2D proj = TextureAssets.Projectile[Type].Value;
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
                    Color finalColor = lightColor * b * (1 - ((float)k / (float)Projectile.oldPos.Length));
                    finalColor.A = 0;//acts like additive blending without spritebatch stuff
                    Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, texture.Size() / 2, new Vector2(a, a), SpriteEffects.None, 0);
                }
            }
            if (Projectile.ai[1] < 160)
            {
                a += 0.05f;
                if (a > 1f)
                    a = 1f;

                b += 0.01f;
                if (b > 0.3f)
                    b = 0.3f;
            }
            else
            {
                a -= 0.006f;
                if (a < 0f)
                    a = 0f;

                b -= 0.006f;
                if (b < 0f)
                    b = 0f;
            }
        
            Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, proj.Size() / 2, new Vector2(a, a), SpriteEffects.None, 0);
            return false;
        }

    }
}

