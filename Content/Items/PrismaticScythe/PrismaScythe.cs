using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.PrismaticScythe
{
    public class PrismaScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 145;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.knockBack = 8;
            Item.value = 27000;
            Item.rare = ItemRarityID.LightPurple;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PrismaScytheS>();
            Item.shootSpeed = 2f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<PrismaScytheS>()] < 1 && player.ownedProjectileCounts[ModContent.ProjectileType<PrismaScytheProj>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                Item.useStyle = 1;
                int basic = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PrismaScytheS>(), damage, knockback, player.whoAmI);
                Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item1, player.position);
                Item.useStyle = ItemUseStyleID.Shoot;
                Projectile.NewProjectile(source, position, velocity * 8f, ModContent.ProjectileType<PrismaScytheProj>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
    public class PrismaScytheS : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 96;
            Projectile.height = 96;
            //Projectile.aiStyle = 1;
            // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
            Projectile.friendly = true;
            //projectile.magic = true;
            //projectile.extraUpdates = 100;
            Projectile.timeLeft = 28; // lowered from 300
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;
            Projectile.light = 1f;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
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
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/PrismaticScythe/PrismaScytheSF").Value;
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
                    Color finalColor = lightColor * 0.1f * (1 - ((float)k / (float)Projectile.oldPos.Length));
                    finalColor.A = 0;//acts like additive blending without spritebatch stuff
                    Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                }
            }
            Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
            float scaleFactor = 80f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
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
            if (Main.rand.NextBool(3))
                Dust.NewDust(hitLineEnd, 5, 5, DustID.PinkFairy, rotationFactor, rotationFactor, 0, default, 0.4f);
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
        int a;
        public override void AI()
        {
            if (dir == Vector2.Zero)
            {
                dir = Main.MouseWorld;
                Projectile.rotation = (MathHelper.PiOver2 * Projectile.ai[1]) - MathHelper.PiOver4 + Projectile.DirectionTo(Main.MouseWorld).ToRotation();
            }
            //FadeInAndOut();
            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.ai[2]++;
            a++;
            Player player = Main.player[Projectile.owner];
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 90);
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
            if (Projectile.ai[2] >= 9 && Projectile.ai[2] <= 16)
                Projectile.scale += 0.07f;
            if (Projectile.ai[2] >= 17 && Projectile.ai[2] <= 21)
                Projectile.scale -= 0.07f;
            if (a == 13)
            {
                SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld) * 14f, ModContent.ProjectileType<PrismaScytheProjShot>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
            }
            Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((10 - Projectile.ai[0])));
            Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3() * 0.5f);
        }
    }
    public class PrismaScytheProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Stars Caller Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 140;
            Projectile.width = 96;
            Projectile.height = 96;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 990;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            Projectile.light = 1f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            if (Projectile.ai[0] >= 76)
                return new Color(248, 133, 235, 0) * Projectile.Opacity;
            else
                return null;
        }
        int timer;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);
            timer++;
            Projectile.ai[0] += 1f;
            if (timer > 30)
            {
                Projectile.rotation -= 0.1f;
            }
            if (timer > 40)
            {
                Projectile.rotation -= 0.2f;
            }
            if (Projectile.ai[0] < 49)
                Projectile.velocity *= 0.95f;
            if (Projectile.ai[0] == 50)
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PrismaGlow>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
            if (Projectile.ai[0] == 76)
            {
                Projectile.velocity = new Vector2(0, -10);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, new Vector2(Main.rand.NextFloat(-7, 7), Main.rand.NextFloat(-7, 7)), ModContent.ProjectileType<PrismaticScytheGore1>(), 0, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, new Vector2(Main.rand.NextFloat(-7, 7), Main.rand.NextFloat(-7, 7)), ModContent.ProjectileType<PrismaticScytheGore2>(), 0, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, new Vector2(Main.rand.NextFloat(-7, 7), Main.rand.NextFloat(-7, 7)), ModContent.ProjectileType<PrismaticScytheGore3>(), 0, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, new Vector2(Main.rand.NextFloat(-7, 7), Main.rand.NextFloat(-7, 7)), ModContent.ProjectileType<PrismaticScytheGore4>(), 0, Projectile.knockBack, Projectile.owner);
            }
            if (Projectile.ai[0] >= 51 && Projectile.ai[0] <= 75)
            {
                Projectile.velocity.X *= 1f + Main.rand.Next(-3, 4) * 0.06f;
                Projectile.velocity.Y *= 1f + Main.rand.Next(-3, 4) * 0.06f;
            }
            if (Projectile.ai[0] >= 83)
            {
                timer = 0;
                float dist = 50;
                float rotTarget = Utils.ToRotation(playerCenter - Projectile.Center);
                float rotCur = Utils.ToRotation(Projectile.velocity);
                float rotMax = MathHelper.ToRadians(7f);
                if (Projectile.Distance(player.Center) <= dist)
                {
                    Projectile.Kill();
                    return;
                }
                Projectile.velocity = Utils.RotatedBy(Projectile.velocity, MathHelper.WrapAngle(MathHelper.WrapAngle(Utils.AngleTowards(rotCur, rotTarget, rotMax)) - Utils.ToRotation(Projectile.velocity)));
            }
            Projectile.rotation += 0.4f;

        }
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
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PinkFairy, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                Main.dust[dust].velocity *= 3f;
            }
        }
    }
    public class PrismaScytheProjShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 90;
            Projectile.width = 84;
            Projectile.height = 124;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 70;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            Projectile.light = 1f;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.95f;
            Projectile.ai[0] += 1f;

            if (Projectile.ai[0] >= 45)
                Projectile.alpha += 20;

            if (Projectile.alpha > 255)
                Projectile.Kill();

            Projectile.rotation = Projectile.velocity.ToRotation();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(226, 197, 168, 0) * Projectile.Opacity;
        }
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
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PinkFairy, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                Main.dust[dust].velocity *= 3f;
            }
        }
    }
    public class PrismaGlow : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.knockBack = 6f;
            Projectile.width = 70;
            Projectile.height = 70;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 26;
            Projectile.light = 1f;
            Projectile.alpha = 255;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(219, 233, 211, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] <= 20)
                Projectile.alpha -= 40;
            Projectile.scale += 0.2f;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
        }
        public override void Kill(int timeLeft)
        {
            Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
            for (int i = 0; i < 8; i++)
            {
                // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<PrismaScytheBeams>(), 60, Projectile.knockBack, Projectile.owner);
            }
            Vector2 velocity = new Vector2(-5, 1);
            for (int i = 0; i < 4; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(360));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(0.2f);
                // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, newVelocity, ModContent.ProjectileType<PrismaGlow2>(), 0, Projectile.knockBack, Projectile.owner);
            }
            for (int i = 0; i < 6; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(360));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(0.2f);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, newVelocity, ModContent.ProjectileType<PrismaGlow3>(), 0, Projectile.knockBack, Projectile.owner);
            }
            PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 6f, 6f, 20, 1000f, FullName);
            Main.instance.CameraModifiers.Add(modifier);
            for (int k = 0; k < 40; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PinkFairy, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                Main.dust[dust].velocity *= 5f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item84, Projectile.position);
        }
    }
    public class PrismaGlow2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 140;
            Projectile.height = 140;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(219, 233, 211, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.95f;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 120)
            {
                Projectile.alpha += 20;
                if (Projectile.alpha > 255)
                    Projectile.Kill();
            }
            else
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 70)
                    Projectile.alpha = 70;
            }

        }
    }
    public class PrismaGlow3 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/PrismaticScythe/PrismaGlow";
        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 70;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(219, 233, 211, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.95f;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 120)
            {
                Projectile.alpha += 20;
                if (Projectile.alpha > 255)
                    Projectile.Kill();
            }
            else
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 80)
                    Projectile.alpha = 80;
            }

        }
    }
    public class PrismaScytheBeams : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Stars Caller Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 60;
            Projectile.knockBack = 6f;
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.light = 1f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return Color.White * Projectile.Opacity;
        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/PrismaticScythe/PrismaScytheBeamsF").Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);

                Color color = Main.DiscoColor * 0.5f * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

                Color color3 = Projectile.GetAlpha(lightColor) * 0.4f * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                Main.EntitySpriteDraw(texture, drawPos, null, color3, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            Color color2 = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, color2, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
            float maxDetectRadius = 600f;
            Projectile.ai[0] += 1f;
            NPC closestNPC = FindClosestNPC(maxDetectRadius);
            if (closestNPC == null)
                return;
            float rotTarget = Utils.ToRotation(closestNPC.Center - Projectile.Center);
            float rotCur = Utils.ToRotation(Projectile.velocity);
            float rotMax = MathHelper.ToRadians(5f);

            // If found, change the velocity of the projectile and turn it in the direction of the target
            // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
            if (Projectile.ai[0] > 20f)
            {
                Projectile.velocity = Utils.RotatedBy(Projectile.velocity, MathHelper.WrapAngle(MathHelper.WrapAngle(Utils.AngleTowards(rotCur, rotTarget, rotMax)) - Utils.ToRotation(Projectile.velocity)));
            }
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
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PinkFairy, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                Main.dust[dust].velocity *= 3f;
            }
        }
    }
}

