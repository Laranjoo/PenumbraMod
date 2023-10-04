using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using System;

namespace PenumbraMod.Content.Items.PrismaBlaster
{
    public class PrismaBlaster : ModItem
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prisma Blaster"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("Hold the gun to fire a huge crystal that in a certain time it explodes in 5 additional fragments" +
                "\nThe fragments explodes on the direction of the mouse, the further the mouse is, the faster they go"
                + "\n'I love prisma!'"); */

        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 52;
            Item.height = 36;
            Item.useTime = 38;
            Item.useAnimation = 38;
            Item.useStyle = 5;
            Item.knockBack = 3;
            Item.value = 11200;
            Item.rare = ItemRarityID.LightPurple;

            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PrismaBlasterProj>();
            Item.shootSpeed = 18f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.crit = 25;
            Item.channel = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<PrismaBlasterProj>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            float launchSpeed = 120f;
            float launchSpeed2 = 20f;
            float launchSpeed3 = 5f;

            Vector2 mousePosition = Main.MouseWorld;
            Vector2 direction = Vector2.Normalize(mousePosition - player.Center);
            Vector2 Gun = direction * launchSpeed2;
            Vector2 Disk = direction * launchSpeed3;
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 80f;
            position = new Vector2(position.X, position.Y);
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position.X, position.Y, Gun.X, Gun.Y, ModContent.ProjectileType<PrismaBlasterGlow>(), 0, knockback, player.whoAmI);
            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position.X, position.Y, Gun.X, Gun.Y, ModContent.ProjectileType<PrismaBlasterProj>(), 0, knockback, player.whoAmI);

            return false; // return false to stop vanilla from calling Projectile.NewProjectile.
        }
    }

    public class PrismaCrystal : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prisma Cystal"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 80;
            Projectile.width = 58;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 40;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity *= 0.94f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
            int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CrystalPulse, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 0.4f);
            Main.dust[dust].velocity *= 2f;
            Projectile.ai[0] += 1f;

            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Lighting.AddLight(Projectile.Center, Color.Pink.ToVector3() * 0.78f);


        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 50; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CrystalPulse, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1.2f);
                Main.dust[dust].velocity *= 6.0f;
            }
            Player player = Main.player[Projectile.owner];
            Vector2 launchVelocity = (Main.MouseWorld - Projectile.Center) / 10f; // Create a velocity moving the center of mouse.
            for (int i = 0; i < 5; i++)
            {
                // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                launchVelocity = launchVelocity.RotatedByRandom(MathHelper.ToRadians(20));

                // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<PrismShardGlow>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.scale *= 1.1f);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<PrismShardWhite>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.scale *= 1.1f);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<PrismShard>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.scale *= 1.1f);
            }


            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item84, Projectile.position);
        }
    }
    public class PrismCrystalWhite : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prisma Cystal"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 40;
            Projectile.alpha = 200;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity *= 0.94f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }

            Projectile.ai[0] += 1f;

            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Lighting.AddLight(Projectile.Center, Color.Pink.ToVector3() * 0.78f);


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
    }
    public class PrismaBlasterProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prisma Blaster"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 76;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 2;
        }
        public int counter;
        public float movement
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/PrismaBlaster/PrismaBlasterProjEf").Value;
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
                    Main.EntitySpriteDraw(texture, lerpedPos, null, Color.White * 0.2f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                }
            }
            return true;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            counter++;
            Vector2 ownedMountedCenter = owner.RotatedRelativePoint(owner.MountedCenter, true);
            Projectile.direction = owner.direction;
            Projectile.timeLeft = 10;
            owner.heldProj = Projectile.whoAmI;
            Projectile.position.X = ownedMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownedMountedCenter.Y - (float)(Projectile.height / 2);
            // As long as the player isn't frozen, the spear can move
            if (!owner.frozen)
            {
                if (movement == 0f) // When initially thrown out, the ai0 will be 0f
                {
                    movement = 0.4f; // Make sure the spear moves forward when initially thrown out
                    Projectile.netUpdate = true; // Make sure to netUpdate this spear
                }
                if (owner.itemAnimation < owner.itemAnimationMax / 3) // Somewhere along the item animation, make sure the spear moves back
                {
                    //movementFactor -= 2.4f;
                }
                else // Otherwise, increase the movement factor
                {
                    //movementFactor += 2.4f;
                }
            }
            Projectile.position += Projectile.velocity * movement;
            // When we reach the end of the animation, we can kill the spear projectile

            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Offset by 90 degrees here
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(180f);
            }
            Projectile.spriteDirection = Projectile.direction;
            if (owner.itemAnimation == 1)
            {
                Projectile.Kill();
            }
            if (counter > 70)
            {
                Vector2 velocity = Projectile.velocity;
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, velocity, ModContent.ProjectileType<PrismGlow>(), 0, 0, owner.whoAmI);
                Vector2 velocity2 = Projectile.velocity;
                int basic2 = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, velocity2, ModContent.ProjectileType<PrismCrystalWhite>(), 0, 0, owner.whoAmI);
                Vector2 velocity3 = Projectile.velocity;
                int basic3 = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, velocity3, ModContent.ProjectileType<PrismaCrystal>(), 70, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.NPCDeath55);
                counter = 0;
            }
            if (!owner.channel || owner.CCed)
            {
                Projectile.timeLeft = 0;
                Projectile.Kill();
                counter = 0;

                return;
            }
        }

    }
    public class PrismaBlasterGlow : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prisma Cystal"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            GameShaders.Armor.GetShaderFromItemId(4778);
        }

        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 76;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 2;
            Projectile.scale = 1.2f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Main.DiscoColor;
        }
        public int counter;
        public float movement
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            counter++;
            Vector2 ownedMountedCenter = owner.RotatedRelativePoint(owner.MountedCenter, true);
            Projectile.direction = owner.direction;
            Projectile.timeLeft = 10;
            owner.heldProj = Projectile.whoAmI;
            Projectile.position.X = ownedMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownedMountedCenter.Y - (float)(Projectile.height / 2);
            // As long as the player isn't frozen, the spear can move
            if (!owner.frozen)
            {
                if (movement == 0f) // When initially thrown out, the ai0 will be 0f
                {
                    movement = 0.4f; // Make sure the spear moves forward when initially thrown out
                    Projectile.netUpdate = true; // Make sure to netUpdate this spear
                }
                if (owner.itemAnimation < owner.itemAnimationMax / 3) // Somewhere along the item animation, make sure the spear moves back
                {
                    //movementFactor -= 2.4f;
                }
                else // Otherwise, increase the movement factor
                {
                    //movementFactor += 2.4f;
                }
            }
            Projectile.position += Projectile.velocity * movement;
            // When we reach the end of the animation, we can kill the spear projectile

            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Offset by 90 degrees here
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(180f);
            }
            Projectile.spriteDirection = Projectile.direction;
            if (owner.itemAnimation == 1)
            {
                Projectile.Kill();
            }

            if (!owner.channel || owner.CCed)
            {
                Projectile.timeLeft = 0;
                Projectile.Kill();
                counter = 0;

                return;
            }
        }

    }
    public class PrismGlow : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prisma Cystal"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            GameShaders.Armor.GetShaderFromItemId(4778);

        }

        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 40;
            Projectile.scale = 1.1f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Main.DiscoColor;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity *= 0.94f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }

            Projectile.ai[0] += 1f;

            Lighting.AddLight(Projectile.Center, Color.Pink.ToVector3() * 0.78f);

            if (Projectile.ai[0] < 1f)
            {
                Vector2 velocity2 = Projectile.velocity;
                int basic2 = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, velocity2, ModContent.ProjectileType<PrismaCrystal>(), 0, 0, player.whoAmI, Projectile.ai[0] + 1);
            }


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
    }
    public class PrismShard : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prism Shard"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 80;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {

            Projectile.rotation += 0.3f;

            Projectile.ai[0] += 1f;
            Lighting.AddLight(Projectile.Center, Color.Pink.ToVector3() * 0.78f);



        }
    }
    public class PrismShardGlow : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prism Shard"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            GameShaders.Armor.GetShaderFromItemId(4778);
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 180;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Main.DiscoColor;
        }

        public override void AI()
        {

            Projectile.rotation += 0.3f;

            Projectile.ai[0] += 1f;
            Lighting.AddLight(Projectile.Center, Color.Pink.ToVector3() * 0.78f);



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
    }
    public class PrismShardWhite : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prism Shard"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 180;
            Projectile.alpha = 200;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * Projectile.Opacity;
        }

        public override void AI()
        {

            Projectile.rotation += 0.3f;

            Projectile.ai[0] += 1f;
            Lighting.AddLight(Projectile.Center, Color.Pink.ToVector3() * 0.78f);


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
    }

}
