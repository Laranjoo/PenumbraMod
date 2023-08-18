using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class AGuitar47 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 120;
            Item.crit = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 66;
            Item.height = 76;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 8;
            Item.value = 1000;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item47;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 12f;
            Item.noMelee = true;

        }
        public override bool AltFunctionUse(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<AguitarBlackHole>()] > 0)
                return false;
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, player.DirectionTo(Main.MouseWorld) * 4f, ModContent.ProjectileType<AguitarBlackHole>(), damage, knockback, player.whoAmI);
            }
            return true;
        }
        public override void AddRecipes()
        {
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 43f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }
    }
    public class AguitarBlackHole : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Icy Shot"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 50;
            Projectile.width = 38;
            Projectile.height = 24;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 0.5f;
            Projectile.penetrate = -1;
        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        bool c = false;
        float x;
        float y;
        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.ai[1]++;
            Projectile.ai[2]++;
            if (Projectile.ai[0] == 1 || Projectile.ai[0] == 20)
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BlackHoleEffect>(), 0, Projectile.knockBack, Projectile.owner);

            if (Projectile.ai[2] == 15)
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BlackHoleEffect2>(), 0, Projectile.knockBack, Projectile.owner);

            if (Projectile.ai[1] >= 1 && Projectile.ai[1] <= 10)
            {
                x += 0.1f;
                y += 0.1f;
            }
            if (Projectile.ai[1] >= 11 && Projectile.ai[1] <= 20)
            {
                x -= 0.1f;
                y -= 0.1f;
            }
            if (Projectile.ai[1] == 29)
                Projectile.ai[1] = 0;

            if (Projectile.ai[0] == 30)
                Projectile.ai[0] = 0;

            Projectile.scale += 0.05f;
            if (Projectile.scale > 1.5f)
            {
                Projectile.scale = 1.5f;
                c = true;
            }

            Projectile.rotation += 0.1f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.friendly)
                    if (npc.Distance(Projectile.Center) < 300f)
                        npc.velocity = npc.DirectionTo(Projectile.Center) * 5f;
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Assets/Textures/StrongGlow").Value;
            Texture2D proj = TextureAssets.Projectile[Projectile.type].Value;
            if (!c)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
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
                    float size = Projectile.scale * (Projectile.oldPos.Length - i) / (Projectile.oldPos.Length);
                    Color finalColor = Projectile.GetAlpha(Color.Black) * (1 - ((float)i / (float)Projectile.oldPos.Length));
                    if (c)
                    {
                        Main.EntitySpriteDraw(texture, lerpedPos + new Vector2(x, y), null, finalColor, lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), size, SpriteEffects.None, 0);
                        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, finalColor, 0, texture.Size() / 2, 3, SpriteEffects.None, 0);
                    }
                    Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                }
            }
            return false;
        }
        /* public override void Kill(int timeLeft)
         {
             Vector2 launchVelocity = new Vector2(-5, 1); // Create a velocity moving the left.
             for (int i = 0; i < 8; i++)
             {
                 // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                 // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                 launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                 // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                 Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<IceSpike>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
             }
             // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
             int dust = Dust.NewDust(Projectile.Center, 2, 1, DustID.IceRod, 0f, 0f, 0);
             Main.dust[dust].noGravity = true;
             Main.dust[dust].velocity *= 6.0f;
             Main.dust[dust].scale = (float)Main.rand.Next(100, 150) * 0.010f;
             Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
             SoundEngine.PlaySound(SoundID.Item27, Projectile.position);

         Saving this for other projectiles
         } */
    }
    public class BlackHoleEffect : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/NPCs/Bosses/Eyestorm/Brightness7";
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.timeLeft = 60;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Black * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.scale += 0.1f;
            Projectile.alpha += 20;
        }
    }
    public class BlackHoleEffect2 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Assets/Textures/StrongGlow-big";
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.timeLeft = 60;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Black * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.scale += 0.1f;
            Projectile.alpha += 20;
        }
    }
}