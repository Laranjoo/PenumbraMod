using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Dusts;
using PenumbraMod.Content.Items.Consumables;
using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class FlyingGume : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Flying Gume"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("Can be only used on 2 directions" +
                "\n''This sword can fly!''"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 30;
            Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 5570;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 2f;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<FlyingGumeProj>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, player.Center, new Vector2(0, 0), ModContent.ProjectileType<FlyingGumeProj>(), 30, 8f, player.whoAmI);
            return false;
        }
        public class FlyingGumeProj : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Flying Gume"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            }

            public override void SetDefaults()
            {
                Projectile.damage = 30;
                Projectile.width = 60;
                Projectile.height = 60;
                Projectile.aiStyle = 0;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.penetrate = -1;
                Projectile.timeLeft = 600;
                Projectile.ignoreWater = false;
                Projectile.tileCollide = false;
                Projectile.light = 0.8f;
            }

         
            public override void AI()
            {
                Player player = Main.player[Projectile.owner];
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] == 1)
                {
                    if (player.direction == 1)
                        Projectile.velocity.X = 24f;
                    else
                        Projectile.velocity.X = -24f;
                }
                if (Projectile.ai[0] < 19)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;
                }
                if (Projectile.ai[0] == 20)
                {
                    Projectile.velocity.X = 0f;
                }
                if (Projectile.ai[0] > 30)
                {
                    Projectile.velocity = Projectile.DirectionTo(player.Center) * 18f;
                    Projectile.rotation += 0.4f;
                    float center = 90f;
                    if (Projectile.DistanceSQ(player.Center) < center)
                    {
                        Projectile.Kill();
                    }
                }
            }
         
            public static float easeInOutQuad(float x)
            {
                return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
            }
            public override bool PreDraw(ref Color lightColor)
            {
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/FlyingGumeProjEf").Value;
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    if (Projectile.oldPos[k] == Vector2.Zero)
                        return false;
                    for (float j = 0; j < 1; j += 0.0625f)
                    {
                        Vector2 lerpedPos;
                        if (k > 0)
                            lerpedPos = Vector2.Lerp(Projectile.oldPos[k - 1], Projectile.oldPos[k], easeInOutQuad(j));
                        else
                            lerpedPos = Vector2.Lerp(Projectile.position, Projectile.oldPos[k], easeInOutQuad(j));
                        float lerpedAngle;
                        if (k > 0)
                            lerpedAngle = Utils.AngleLerp(Projectile.oldRot[k - 1], Projectile.oldRot[k], easeInOutQuad(j));
                        else
                            lerpedAngle = Utils.AngleLerp(Projectile.rotation, Projectile.oldRot[k], easeInOutQuad(j));
                        lerpedPos += Projectile.Size / 2;
                        lerpedPos -= Main.screenPosition;
                        Main.EntitySpriteDraw(texture, lerpedPos, null, Color.White * 0.3f * (1 - ((float)k / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                    }
                }
                return true;
            }
        }
    }
}