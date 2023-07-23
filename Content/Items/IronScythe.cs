using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Consumables;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class IronScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Iron Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("Big and sharp!" +
                "\nSpins the scythe through player" +
				"\n[c/d1d8d9:Special ability:] When used, The player throws 2 Iron scythes"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 72;
			Item.height = 57;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = 3450;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IronScytheSwing>();
            Item.shootSpeed = 1f;
            Item.noUseGraphic = false;
            Item.noMelee = true;
        }
        bool notboollol = true;
        public int TwistedStyle = 0;
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noUseGraphic = true;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = false;
            }
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[ModContent.ProjectileType<IronScytheProj>()] < 1;
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

                int basic = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<IronScytheSwing>(), damage, knockback, player.whoAmI);
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
                if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
                {
                    TwistedStyle = 1;
                    Item.useStyle = ItemUseStyleID.Shoot;
                    Item.noUseGraphic = true;
                }
            }
            if (TwistedStyle == 1)
            {
                const int NumProjectiles = 2;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(30));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 14f - Main.rand.NextFloat(0f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<IronScytheProj>(), damage, knockback, player.whoAmI);
                }
                TwistedStyle = 0;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noUseGraphic = true;
            }
            else if (TwistedStyle == 2)
            {
                TwistedStyle = 0;
            }
            return false; // return false to stop vanilla from calling Projectile.NewProjectile.
        }
        int TwistedTimer = 0;

        public override Nullable<bool> UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (TwistedStyle == 2)
            {
                Item.noUseGraphic = true;
                TwistedTimer++;
                TwistedStyle++;
                if (TwistedStyle > 1)
                {
                    TwistedStyle = 0;
                    TwistedTimer = 0;
                }
                //Item.shoot = TwistedStyle + 120;
                //Main.NewText($"Switching to ItemUseStyleID #{Item.useStyle}");
            }
            else
            {
                //Main.NewText($"This is ItemUseStyleID #{Item.useStyle}");
            }
            return true;
        }
        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.IronBar, 17)
			.AddIngredient(ItemID.Wood, 12)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
        public class IronScytheSwing : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Iron Scythe");
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            }

            public override void SetDefaults()
            {
                Projectile.width = 121;
                Projectile.height = 108;
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

            public static float easeInOutQuad(float x)
            {
                return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
            }
            public override bool PreDraw(ref Color lightColor)
            {
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/IronScytheSwingef").Value;

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
                        Main.EntitySpriteDraw(texture, lerpedPos, null, Color.White * 0.2f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 0);
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
                Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((40 - Projectile.ai[0])));
                Player player = Main.player[Projectile.owner];
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 90);
                if (player.ownedProjectileCounts[ModContent.ProjectileType<IronScytheProj>()] >= 1)
                {
                    Projectile.Kill();
                    Projectile.alpha = 255;
                    Projectile.active = false;
                }
            }

        }
    }
}