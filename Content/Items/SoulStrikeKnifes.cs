using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using PenumbraMod.Content.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.DataStructures;
using PenumbraMod.Content.Dusts;

namespace PenumbraMod.Content.Items
{
    public class SoulStrikeKnifes : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Magic;
            Item.width = 18;
            Item.height = 36;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.value = 1000;
            Item.rare = 2;
            Item.UseSound = SoundID.Item39;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SoulStrikeKnife>();
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.crit = 16;
            Item.noUseGraphic = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float NumProjectiles = Main.rand.NextFloat(2, 4);
            for (float i = 0; i < NumProjectiles; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(20));
                // Create a projectile.
                Projectile.NewProjectileDirect(source, position, newVelocity, type, Item.damage, knockback, player.whoAmI);
            }
            return true;

        }
    }
    public class SoulStrikeKnife : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.width = 18;
            Projectile.height = 26;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type != NPCID.TargetDummy)
            {
                Player player = Main.player[Projectile.owner];
                player.ManaEffect(damageDone);
                player.statMana += damageDone;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.Center, Projectile.DirectionTo(player.Center) * 2f, ModContent.ProjectileType<SoulStrikeKnifeHeal>(), 0, 0, Projectile.owner);
            }
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
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.78f);
        }
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueTorch, Projectile.oldVelocity.X * 1f, Projectile.oldVelocity.Y * 1f, Scale: 1f);
            } 
        }

    }
    public class SoulStrikeKnifeHeal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cosmic Bullet"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
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
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.78f);
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDustPerfect(Projectile.position, ModContent.DustType<BlueGlow>(), Vector2.Zero, 0, default, Scale: 1f);
            }
            Player player = Main.player[Projectile.owner];
            Projectile.ai[0] += 1f;
            float projSpeed = 45;
            float dist = 50;
            Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
            if (Projectile.Distance(player.Center) <= dist)
            {
                Projectile.Kill();
                return;
            }
        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
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
                    Main.EntitySpriteDraw(texture, lerpedPos, null, color * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                }
            }
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(0, 234, 255, 0) * Projectile.Opacity;
        }
    }
}