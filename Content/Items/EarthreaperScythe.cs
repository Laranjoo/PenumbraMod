using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Placeable;
using System.IO;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Drawing;
using ReLogic.Content;

namespace PenumbraMod.Content.Items
{
    public class EarthreaperScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 245;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 52;
            Item.height = 58;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = 1;
            Item.knockBack = 5;
            Item.value = 40170;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<EarthreaperScytheSwing>();
            Item.shootSpeed = 2f;
            Item.autoReuse = true;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useTurn = false;
        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[ModContent.ProjectileType<EarthreaperScytheSwing>()] < 1 || player.ownedProjectileCounts[ModContent.ProjectileType<EarthreaperScytheSwing2>()] < 1;
        }
        int SwingCount = 0;
        bool notboollol = true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (SwingCount == 0)
            {
                int basic = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (notboollol == true)
                {
                    Main.projectile[basic].ai[2] = -1;
                    notboollol = false;
                }
                else
                {
                    Main.projectile[basic].ai[2] = 1;
                    notboollol = true;
                }
                Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
                SwingCount = 1;
            }
            else if (SwingCount == 1)
            {
                int basic = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (notboollol == true)
                {
                    Main.projectile[basic].ai[2] = -1;
                    notboollol = false;
                }
                else
                {
                    Main.projectile[basic].ai[2] = 1;
                    notboollol = true;
                }
                Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
                SwingCount = 2;
            }
            else if (SwingCount == 2)
            {
                int basic = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (notboollol == true)
                {
                    Main.projectile[basic].ai[2] = -1;
                    notboollol = false;
                }
                else
                {
                    Main.projectile[basic].ai[2] = 1;
                    notboollol = true;
                }
                Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
                SwingCount = 3;
            }
            else if (SwingCount == 3)
            {
                int basic = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<EarthreaperScytheSwing2>(), damage, knockback, player.whoAmI);
                if (notboollol == true)
                {
                    Main.projectile[basic].ai[2] = -1;
                    notboollol = false;
                }
                else
                {
                    Main.projectile[basic].ai[2] = 1;
                    notboollol = true;
                }
                Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
                SwingCount = 0;
            }

            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodystoneScythe>(), 1);
            recipe.AddIngredient(ModContent.ItemType<CorrosiveScythe>(), 1);
            recipe.AddIngredient(ItemID.IceSickle);
            recipe.AddIngredient(ModContent.ItemType<DeathstrandingScythe>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<InfectedScythe>(), 1);
            recipe2.AddIngredient(ModContent.ItemType<CorrosiveScythe>(), 1);
            recipe2.AddIngredient(ItemID.IceSickle);
            recipe2.AddIngredient(ModContent.ItemType<DeathstrandingScythe>(), 1);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.Register();
        }
    }
    public class EarthreaperScytheSwing : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 104;
            Projectile.height = 116;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
        }
        Vector2 dir = Vector2.Zero;
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
            float scaleFactor = 90f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
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
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/EarthreaperScytheSwingef").Value;

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
                    if (Projectile.ai[2] == -1)
                        Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle - 0.78f, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                    if (Projectile.ai[2] == 1)
                        Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle - 0.78f, texture.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
                }
            }

            if (Projectile.ai[2] == -1)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            if (Projectile.ai[2] == 1)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);

            return false;
        }
        int a;
        public override void AI()
        {
            if (dir == Vector2.Zero)
            {
                dir = Main.MouseWorld;
                Projectile.rotation = (MathHelper.PiOver2 * Projectile.ai[2]) - MathHelper.PiOver4 + Projectile.DirectionTo(Main.MouseWorld).ToRotation();
            }
            //FadeInAndOut();
            Projectile.Center = Main.player[Projectile.owner].Center;
            a++;
            Player player = Main.player[Projectile.owner];
            player.SetDummyItemTime(2);
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (Projectile.rotation + 0.78f) + 90f);
            if (Projectile.ai[2] <= 8)
            {
                Projectile.ai[0] += 0.1f;
                Projectile.ai[1] += 0.1f;
            }
            else
            {
                Projectile.ai[0] += 0.2f;
                Projectile.ai[1] += 0.2f;
            }
            if (a >= 9 && a <= 15)
                Projectile.scale += 0.1f;
            if (a >= 15 && a <= 20)
                Projectile.scale -= 0.2f;

            if (a == 10)
            {
                SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, player.DirectionTo(Main.MouseWorld) * 20f, ModContent.ProjectileType<EarthreaperScytheBeam>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
               

            if (Projectile.ai[2] == 1)
                Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((10 - Projectile.ai[0])));
            if (Projectile.ai[2] == -1)
                Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((-10 - Projectile.ai[0])));
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Vanilla has several particles that can easily be used anywhere.
            // The particles from the Particle Orchestra are predefined by vanilla and most can not be customized that much.
            // Use auto complete to see the other ParticleOrchestraType types there are.
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade,
                new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
                Projectile.owner);

            // Set the target's hit direction to away from the player so the knockback is in the correct direction.
            hit.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade,
                new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
                Projectile.owner);

            info.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
        }
    }
    public class EarthreaperScytheSwing2 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/EarthreaperScytheSwing";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 104;
            Projectile.height = 116;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
        }
        Vector2 dir = Vector2.Zero;
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
            float scaleFactor = 105f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
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
            Dust.NewDust(hitLineEnd, 0, 0, DustID.TerraBlade);

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
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/EarthreaperScytheSwingef").Value;

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
                    if (Projectile.ai[2] == -1)
                        Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle - 0.78f, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                    if (Projectile.ai[2] == 1)
                        Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle - 0.78f, texture.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
                }
            }

            if (Projectile.ai[2] == -1)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            if (Projectile.ai[2] == 1)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
            return false;
        }
        int a;
        public override void AI()
        {
            if (dir == Vector2.Zero)
            {
                dir = Main.MouseWorld;
                Projectile.rotation = (MathHelper.PiOver2 * Projectile.ai[2]) - MathHelper.PiOver4 + Projectile.DirectionTo(Main.MouseWorld).ToRotation() - 2.1f;
            }
            Projectile.Center = Main.player[Projectile.owner].Center;
            a++;
            Player player = Main.player[Projectile.owner];
            player.SetDummyItemTime(2);
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (Projectile.rotation + 0.78f) + 90f);
            if (a < 8)
            {
                Projectile.ai[0] += 0.05f;
                Projectile.ai[1] += 0.05f;               
            }
            else
            {
                Projectile.ai[0] += 0.3f;
                Projectile.ai[1] += 0.3f;
            }
            if (a >= 9 && a <= 15)
                Projectile.scale += 0.2f;
            if (a >= 15 && a <= 20)
                Projectile.scale -= 0.3f;

            if (a == 12)
            {
                SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, player.DirectionTo(Main.MouseWorld) * 32f, ModContent.ProjectileType<EarthreaperScytheBeam2>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, player.DirectionTo(Main.MouseWorld) * 32f, ModContent.ProjectileType<EarthreaperScytheBeam2eff>(), 0, 0, Projectile.owner);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, player.DirectionTo(Main.MouseWorld) * 32f, ModContent.ProjectileType<EarthreaperScytheBeam2ef>(), 0, 0, Projectile.owner);
            }

            if (Projectile.ai[2] == 1)
                Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((15 - Projectile.ai[0])));
            if (Projectile.ai[2] == -1)
                Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((-15 - Projectile.ai[0])));
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Vanilla has several particles that can easily be used anywhere.
            // The particles from the Particle Orchestra are predefined by vanilla and most can not be customized that much.
            // Use auto complete to see the other ParticleOrchestraType types there are.
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade,
                new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
                Projectile.owner);

            // Set the target's hit direction to away from the player so the knockback is in the correct direction.
            hit.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade,
                new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
                Projectile.owner);

            info.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
        }
    }
}