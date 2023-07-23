using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class Wakizashi : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wakizashi"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            // Tooltip.SetDefault("Ancient ones used that");

        }

        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = (DamageClass.Melee);
            Item.width = 71;
            Item.height = 71;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.knockBack = 2;
            Item.value = 453450;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<WakizashiSwing>();
            Item.shootSpeed = 2f;
            Item.noUseGraphic = false;
            Item.scale = 2f;
            Item.noMelee = true;
        }

        bool notboollol = true;
        public int TwistedStyle = 0;
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Wakizashi1>()] < 1)
            {//Equip animation.
                int index = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<Wakizashi1>(), 0, 0, player.whoAmI, 0f);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Item.noUseGraphic = true;
            if (TwistedStyle == 0)
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

                int basic = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<WakizashiSwing>(), damage, knockback, player.whoAmI);
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
        // return false to stop vanilla from calling Projectile.NewProjectile.

        public class WakizashiSwing : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Wakizashi");
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            }

            public override void SetDefaults()
            {
                Projectile.damage = 200;
                Projectile.width = 272;
                Projectile.height = 284;
                //Projectile.aiStyle = 1;
                // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
                Projectile.friendly = true;
                //projectile.magic = true;
                //projectile.extraUpdates = 100;
                Projectile.timeLeft = 20; // lowered from 300
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.knockBack = 8f;
                Projectile.netImportant = true;
            }
            public static float easeInOutQuad(float x)
            {
                return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
            }
            public override bool PreDraw(ref Color lightColor)
            {
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
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
                        Color finalColor = lightColor * 0.1f * (1 - ((float)k / (float)Projectile.oldPos.Length));
                        finalColor.A = 0;//acts like additive blending without spritebatch stuff
                        Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, texture.Size() / 2, 1, SpriteEffects.None, 0);
                    }
                }
                return true;
            }
            Vector2 dir = Vector2.Zero;
            Vector2 hlende = Vector2.Zero;
            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
                float scaleFactor = 190f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
                float widthMultiplier = 23f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
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

                hlende = hitLineEnd;
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
                Projectile.Center = Main.player[Projectile.owner].Center;
                Projectile.ai[0] += 1f;
                Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((20 - Projectile.ai[0])));
                
            }

        }
        public class Wakizashi1 : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Wakizashi");
                Main.projFrames[Projectile.type] = 2;
            }
            public override void SetDefaults()
            {

                AIType = 0;

                Projectile.width = 71;
                Projectile.height = 71;
                Projectile.timeLeft = 60;
                Projectile.penetrate = -1;
                Projectile.light = 0.3f;
                Projectile.hide = false;
                Projectile.alpha = 0;
                Projectile.netImportant = true;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                Projectile.rotation = 340;
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

                    Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(180f);
                }
                else
                {

                    Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(180f);
                }
                //Projectile.Center += projOwner.gfxOffY * Vector2.UnitY;//Prevent glitchy animation.

                Projectile.ai[0]++;

                if (projOwner.dead && !projOwner.active)
                {//Disappear when player dies
                    Projectile.timeLeft = 0;
                    Projectile.Kill();
                    Projectile.alpha = 255;
                }
                if (projOwner.HeldItem?.type != ModContent.ItemType<Wakizashi>())
                {
                    Projectile.Kill();
                }
                if (projOwner.ownedProjectileCounts[ModContent.ProjectileType<WakizashiSwing>()] >= 1)
                {

                    Projectile.frame = 1;
                }
                else
                {
                    //Arms will hold the weapon.
                    projOwner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (projOwner.Center -
                        new Vector2(Projectile.Center.X + (projOwner.velocity.X * 0.05f), Projectile.Center.Y + (projOwner.velocity.Y * 0.05f))
                        ).ToRotation() + MathHelper.PiOver2);
                    Projectile.frame = 0;
                }

                //Orient projectile
                Projectile.direction = projOwner.direction;
            }
        }

    }
}

