using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Dusts;
using PenumbraMod.Content.Items.Consumables;
using PenumbraMod.Content.Items.PrismaticScythe;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class DeathstrandingScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Deathstranding Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            // Tooltip.SetDefault("[c/5e5e5e:Special ability:] When used, the scythe makes a blue explosion, damaging monsters nearby, and also gives 5+ defense for 5 seconds");
        }

		public override void SetDefaults()
		{
			Item.damage = 72;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 92;
			Item.height = 70;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 23400;
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<EMPTY>();
			Item.shootSpeed = 16f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
			{
				SoundEngine.PlaySound(SoundID.Item71, player.position);
                Item.noUseGraphic = true;
                Item.noMelee = true;
				// Create a projectile.
				Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<DeathStrandingSpecial>(), damage, knockback, player.whoAmI);
			}
            else
            {
                Item.noMelee = false;
                Item.noUseGraphic = false;
            }

            return true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<DeathStrandingSpecial>()] < 1;
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