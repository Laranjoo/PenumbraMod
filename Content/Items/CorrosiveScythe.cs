using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class CorrosiveScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 82;
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
                if (player.velocity.Y > 5)
                {
                    if (player.direction == 0)
                        notboollol = true;
                    else
                        notboollol = false;
                }
                if (player.velocity.Y < -5)
                {
                    if (player.direction != 0)
                        notboollol = true;
                    else
                        notboollol = false;
                }
                int basic = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CorrosiveScytheSwing>(), damage, knockback, player.whoAmI);
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
                if (player.velocity.Y > 5)
                {
                    if (player.direction == 0)
                        notboollol = true;
                    else
                        notboollol = false;
                }
                if (player.velocity.Y < -5)
                {
                    if (player.direction != 0)
                        notboollol = true;
                    else
                        notboollol = false;
                }

                int basic = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CorrosiveScytheSwing2>(), damage, knockback, player.whoAmI);
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
                player.velocity = player.DirectionTo(Main.MouseWorld) * 16f;

                const int NumProjectiles = 2;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                    float ceilingLimit = target.Y;
                    if (ceilingLimit > player.Center.Y - 200f)
                    {
                        ceilingLimit = player.Center.Y - 200f;
                    }
                    // Loop these functions 20 times.
                    for (int e = 0; e < 20; e++)
                    {
                        position = player.Center - Utils.RandomVector2(Main.rand, 1200, 0) * player.direction + new Vector2(0, 600f);
                        position.Y -= 100 * e;
                        Vector2 heading = target - position;

                        if (heading.Y < 0f)
                        {
                            heading.Y *= -1f;
                        }

                        if (heading.Y < 20f)
                        {
                            heading.Y = 20f;
                        }

                        heading.Normalize();
                        heading *= velocity.Length();
                        heading.Y += Main.rand.Next(-40, 45) * 0.02f;
                        Projectile.NewProjectile(source, position, heading, ModContent.ProjectileType<CorrosiveShot>(), damage * 2, knockback, player.whoAmI, 0f, ceilingLimit);
                    }

                }

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
            Projectile.width = 160;
            Projectile.height = 140;
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
                    Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, texture.Size() / 2, 1, SpriteEffects.None, 0);
                }
            }
            return true;
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
            //FadeInAndOut();
            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.ai[0] += 1f;
            Player player = Main.player[Projectile.owner];
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 90);
            Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((20 - Projectile.ai[0])));
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 0.5f);
        }

    }
    public class CorrosiveScytheSwing2 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/CorrosiveScytheSwing";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 140;
            //Projectile.aiStyle = 1;
            // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
            Projectile.friendly = true;
            //projectile.magic = true;
            //projectile.extraUpdates = 100;
            Projectile.timeLeft = 30; // lowered from 300
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            Projectile.netImportant = true;
        }
        Vector2 dir = Vector2.Zero;
        Vector2 hlende = Vector2.Zero;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
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
                    Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, texture.Size() / 2, 1, SpriteEffects.None, 0);
                }
            }
            return true;
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
            // All Projectiles have timers that help to delay certain events
            // Projectile.ai[0], Projectile.ai[1] � timers that are automatically synchronized on the client and server
            // Projectile.localAI[0], Projectile.localAI[0] � only on the client
            // In this example, a timer is used to control the fade in / out and despawn of the Projectile
            //Projectile.ai[0] += 1f;
            if (dir == Vector2.Zero)
            {
                dir = Main.MouseWorld;
                Projectile.rotation = (MathHelper.PiOver2 * Projectile.ai[1]) - MathHelper.PiOver4 + Projectile.DirectionTo(Main.MouseWorld).ToRotation();
            }
            //FadeInAndOut();
            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.ai[0] += 1f;
            Player player = Main.player[Projectile.owner];
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 90);
            Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((40 - Projectile.ai[0])));
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 0.5f);
        }

    }
    public class CorrosiveShot : ModProjectile
    {
        public override string Texture => "PenumbraMod/EMPTY";
        public override void SetDefaults()
        {
            Projectile.damage = 72;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 6000;
            Projectile.light = 0.20f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 8;
        }

        public override void AI()
        {
            int dust2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.GreenTorch, 3f, 0f, 0);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 1.2f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 135) * 0.012f;
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 0.3f);
        }
        public override void Kill(int timeLeft)
        {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Corrosion>(), 300);
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
                || projOwner.ownedProjectileCounts[ModContent.ProjectileType<CorrosiveScytheSwing2>()] >= 1 || projOwner.channel)
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
}

