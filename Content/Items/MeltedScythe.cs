
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Dusts;
using PenumbraMod.Content.Items.Consumables;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace PenumbraMod.Content.Items
{
    public class MeltedScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("Hot and explosive!" +
                "\nWhen you get a critical strike, the scythe does an explosion on enemies" +
                "\n[c/ff4100:Special ability:] When used, the player spins the scythe and shoots fireballs"); */

        }

        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 70;
            Item.height = 60;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.value = 8580;
            Item.rare = 4;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MeltedScytheSwing>();
            Item.shootSpeed = 1f;
            Item.noUseGraphic = false; ;
            Item.crit = 23;
        }
        bool notboollol = true;
        public int TwistedStyle = 0;
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = true;
            }
            else
            {
                Item.useAnimation = 26;
                Item.useTime = 26;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = false;
            }
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[ModContent.ProjectileType<MeltedScytheSwing>()] < 1;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 120);
            Hit(player, target, hit);
        }
        void Hit(Player player, NPC target, NPC.HitInfo hit)
        {
            if (hit.Crit)
            {
                Vector2 newVelocity = target.velocity.RotatedByRandom(MathHelper.ToRadians(0));
                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 8f - Main.rand.NextFloat(0.1f);
                Projectile.NewProjectile(target.GetSource_FromThis(), target.position, newVelocity, ModContent.ProjectileType<MeltedShotEx>(), 50, 12, player.whoAmI);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                if (TwistedStyle == 0)
                {
                    Item.useTime = 40;
                    Item.useAnimation = 40;

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
                    const int NumProjectiles = 8;
                    for (int i = 0; i < NumProjectiles; i++)
                    {
                        Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(210));
                        // Decrease velocity randomly for nicer visuals.
                        newVelocity *= 8f - Main.rand.NextFloat(0.1f);
                        // Create a projectile.
                        Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<MeltedShot>(), damage, knockback, player.whoAmI);

                    }
                    int basic = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MeltedScytheSwing>(), damage, knockback, player.whoAmI);
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
            }

            return false; // return false to stop vanilla from calling Projectile.NewProjectile.
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<MeltedEmber>(), 15)
            .AddIngredient(ItemID.Obsidian, 18)
            .AddIngredient(ItemID.SoulofNight, 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
        public class MeltedScytheSwing : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Melted Scythe");
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 4;
            }

            public override void SetDefaults()
            {
                Projectile.width = 140;
                Projectile.height = 120;
                //Projectile.aiStyle = 1;
                // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
                Projectile.friendly = true;
                //projectile.magic = true;
                //projectile.extraUpdates = 100;
                Projectile.timeLeft = 40; // lowered from 300
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.ownerHitCheck = true;
                Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
                Projectile.netImportant = true;
            }
            Vector2 dir = Vector2.Zero;
            Vector2 hlende = Vector2.Zero;

            public override bool PreDraw(ref Color lightColor)
            {
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

                // Redraw the projectile with the color not influenced by light
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                }

                return true;
            }
            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
                float scaleFactor = 130f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
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
                Player player = Main.player[Projectile.owner];
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 90);
                //FadeInAndOut();
                Projectile.Center = Main.player[Projectile.owner].Center;
                Projectile.ai[0] += 1f;
                Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((40 - Projectile.ai[0])));
            }

        }
        public class MeltedShot : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("MeltedShot"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.

            }

            public override void SetDefaults()
            {
                Projectile.damage = 25;
                Projectile.width = 12;
                Projectile.height = 12;
                Projectile.aiStyle = 68;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.penetrate = 3;
                Projectile.timeLeft = 600;
                Projectile.light = 0.25f;
                Projectile.ignoreWater = false;
                Projectile.tileCollide = true;
                Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                target.AddBuff(BuffID.OnFire, 120);
            }
            public override void AI()
            {
                int dust2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Lava, 4f, 4f, 0);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].velocity *= 2.6f;
                Main.dust[dust2].scale = (float)Main.rand.Next(150, 200) * 0.006f;
                Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.80f);
            }

            public override void Kill(int timeLeft)
            {
                // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
        }
        public class MeltedShotEx : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("MeltedShotEx"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.

            }

            public override void SetDefaults()
            {
                Projectile.damage = 30;
                Projectile.width = 12;
                Projectile.height = 12;
                Projectile.aiStyle = 68;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.penetrate = 1;
                Projectile.timeLeft = 40;
                Projectile.light = 0.25f;
                Projectile.ignoreWater = false;
                Projectile.tileCollide = false;
                Projectile.hide = false;
                Projectile.alpha = 0;
                Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                target.AddBuff(BuffID.OnFire, 120);
                target.AddBuff(ModContent.BuffType<MeltedEx>(), 5);
            }
            public override void AI()
            {

                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Lava, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1.6f);
                Main.dust[dust].velocity *= 6.0f;
                // Trying to find NPC closest to the projectile
                float maxDetectRadius = 600f; // The maximum radius at which a projectile can detect a target
                float projSpeed = 1364f; // The speed at which the projectile moves towards the target

                // Trying to find NPC closest to the projectile
                NPC closestNPC = FindClosestNPC(maxDetectRadius);
                if (closestNPC == null)
                {
                    Projectile.alpha += 60;
                    if (Projectile.alpha > 255)
                    {
                        Projectile.Kill();
                    }
                    return;
                }

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
            public override void Kill(int timeLeft)
            {
                for (int k = 0; k < 30; k++)
                {
                    int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Lava, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1.6f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item70, Projectile.position);
            }
        }
    }
}