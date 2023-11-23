using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Dusts;
using PenumbraMod.Content.Items.Consumables;
using PenumbraMod.Content.Items.PrismaticScythe;
using System;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class DeathstrandingScythe : ModItem
	{

		public override void SetDefaults()
		{
			Item.damage = 72;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 92;
			Item.height = 70;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 23400;
			Item.rare = 4;
            Item.channel = true;
			Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
			Item.shootSpeed = 2f;
            Item.useTurn = false;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
			{
				SoundEngine.PlaySound(SoundID.Item71, player.position);             
				// Create a projectile.
				Projectile.NewProjectileDirect(source, position, velocity * 8f, ModContent.ProjectileType<DeathStrandingSpecial>(), damage, knockback, player.whoAmI);
			}
            else
            {
                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<DeathstrandingScytheS>(), damage, knockback, player.whoAmI);
            }

            return true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<DeathStrandingSpecial>()] < 1;
        }
    }
    public class DeathstrandingScytheS : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            //ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 140;
            Projectile.height = 160;
            Projectile.friendly = false;
            Projectile.timeLeft = 9999;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            Projectile.netImportant = true;
        }
        Vector2 dir = Vector2.Zero;
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
            float scaleFactor = 105f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
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
            if (Projectile.friendly
                && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier, ref collisionPoint))
            {
                return true;
            }
            return false;
        }
        int charge;
        float col = 0;
        bool hasswung = false;
        public override void AI()
        {
            Projectile.Center = Main.player[Projectile.owner].Center;
            Player player = Main.player[Projectile.owner];         
            player.SetDummyItemTime(2);
            if (player.noItems || player.CCed || player.dead || !player.active)
                Projectile.Kill();
            if (dir == Vector2.Zero)
            {
                dir = Main.MouseWorld;
                Projectile.rotation = (MathHelper.PiOver2 * Projectile.ai[1]) - MathHelper.PiOver4 + Projectile.DirectionTo(Main.MouseWorld).ToRotation();
            }
            //FadeInAndOut();
            if (player.direction == 1)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (Projectile.rotation + 0.78f) + 90f);
                Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((11 - Projectile.ai[0])));
            }
               
            if (player.direction == -1)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (Projectile.rotation - 0.78f) + 90f);
                Projectile.rotation -= (Projectile.ai[1] * MathHelper.ToRadians((11 - Projectile.ai[0])));
            }

            if (Main.mouseLeft && !hasswung)
                Charge(player);
            else
                Swing(player);
        }
        bool Charge(Player player)
        {
            if (Main.mouseLeft && Projectile.ai[2] >= 2)
                return true;
            if (charge >= 101)
            {
                col = 1f;
                return true;
            }
            if (charge == 100)
            {
                SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
                for (int i = 0; i < 30; i++)
                {
                    int d = Dust.NewDust(player.position, player.width, player.height, DustID.BlueTorch);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 8f;
                }
           
            }
            if (charge <= 99 && !Main.mouseLeft)
            {
                hasswung = true;
                return true;
            }
               
            charge++;

            if (charge > 50)
            {
                if (Main.rand.NextBool(3))
                {
                    int d = Dust.NewDust(player.position, 30, player.height, DustID.BlueTorch);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity.Y -= 8f;
                }
                col += 0.02f;
            }
            return false;
        }
        void Swing(Player player)
        {
            if (!Charge(player))
                return;
            hasswung = true;
            Projectile.ai[2]++;
            if (Projectile.ai[2] >= 1 && Projectile.ai[2] <= 12)
            {
                Projectile.ai[0] -= 0.03f;
                Projectile.ai[1] -= 0.03f;
                player.channel = false;
                col -= 0.05f;
            }
            if (Projectile.ai[2] >= 13 && Projectile.ai[2] <= 21)
            {
                Projectile.friendly = true;
                Projectile.ai[0] += 1f;
                Projectile.ai[1] += 1f;             
                player.channel = false;
            }
            if (Projectile.ai[2] == 16)
            {
                SoundEngine.PlaySound(SoundID.Item71, Projectile.position);
                if (charge >= 100)
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld) * 12f, ModContent.ProjectileType<DeathExplosion>(), Projectile.damage * 2, Projectile.knockBack, player.whoAmI);
            }
                
            if (Projectile.ai[2] >= 22 && Projectile.ai[2] <= 25)
            {
                Projectile.ai[0] -= 0.3f;
                Projectile.ai[1] -= 0.3f;
                player.channel = false;
            }
            if (Projectile.ai[2] > 26)
                Projectile.Kill();

        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/DeathstrandingScytheSF").Value;
            Player player = Main.player[Projectile.owner];

            if (player.direction == 1)
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White * col, Projectile.rotation - 0.78f, texture.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
            if (player.direction == -1)
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White * col, Projectile.rotation - 0.78f, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

            if (player.direction == 1)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
            if (player.direction == -1)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
    public class DeathStrandingSpecial : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/DeathstrandingScythe";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

        }
        public override void SetDefaults()
        {
            Projectile.damage = 75;
            Projectile.width = 92;
            Projectile.height = 70;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
            Projectile.netImportant = true;
        }
        int t;
        bool checkGettingBack = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] == 68)
            {
                SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<DeathExplosion>(), Projectile.damage * 2, Projectile.knockBack, player.whoAmI);
                PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 6f, 6f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
                for (int i = 0; i < 20; i++)
                {
                    int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueTorch, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                    Main.dust[dust].velocity *= 8f;
                    Main.dust[dust].color = Color.LightBlue;
                }
                for (int i = 0; i < 3; i++)
                {
                    Vector2 velocity = new Vector2(-8, 0).RotatedByRandom(360);
                    velocity *= 1f - Main.rand.NextFloat(0.4f);
                    Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, velocity, ModContent.ProjectileType<DeathExplosion2>(), 0, 0, Projectile.owner);
                }
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, new Vector2(Main.rand.NextFloat(-7, 7), Main.rand.NextFloat(-7, 7)), ModContent.ProjectileType<DeathstrandingScytheGore1>(), 0, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, new Vector2(Main.rand.NextFloat(-7, 7), Main.rand.NextFloat(-7, 7)), ModContent.ProjectileType<DeathstrandingScytheGore2>(), 0, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, new Vector2(Main.rand.NextFloat(-7, 7), Main.rand.NextFloat(-7, 7)), ModContent.ProjectileType<DeathstrandingScytheGore3>(), 0, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, new Vector2(Main.rand.NextFloat(-7, 7), Main.rand.NextFloat(-7, 7)), ModContent.ProjectileType<DeathstrandingScytheGore4>(), 0, Projectile.knockBack, Projectile.owner);
            }
            if (Projectile.ai[0] > 70)
            {
                Projectile.netUpdate = true;
                Projectile.aiStyle = 0;
                Projectile.extraUpdates = 1;
                checkGettingBack = true;
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
            Projectile.rotation += 0.1f;
        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        float d;
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/DeathstrandingScythe").Value;
            Color finalColor = Color.LightBlue;
            finalColor.A = 0;//acts like additive blending without spritebatch stuff
            d += 0.1f;
            if (!checkGettingBack)
            Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor * d, Projectile.rotation, proj.Size() / 2, 1, SpriteEffects.None, 0);
            else
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, finalColor, Projectile.rotation, proj.Size() / 2, 1, SpriteEffects.None, 0);
            return false;
        }
    }
    public class DeathExplosion : ModProjectile
    {
        public override string Texture => "PenumbraMod/Assets/Textures/FireTextures/FireTextureBG";
        public override void SetDefaults()
        {
            Projectile.damage = 150;
            Projectile.width = 325;
            Projectile.height = 321;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 243, 255, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.alpha += 5;
            if (Projectile.alpha > 255)
                Projectile.Kill();
            Projectile.rotation += 0.1f;
        }
    }
    public class DeathExplosion2 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Assets/Textures/FireTextures/FireTexture-BitSmallBG";
        public override void SetDefaults()
        {
            Projectile.width = 162;
            Projectile.height = 160;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
            Projectile.alpha = 140;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 243, 255, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.alpha += 5;
            Projectile.rotation += 0.1f;
        }
    }
}