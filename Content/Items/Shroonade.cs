using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace PenumbraMod.Content.Items
{
    public class Shroonade : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 20;
            Item.height = 26;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8;
            Item.value = 5500;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ShroonadeThrown>();
            Item.shootSpeed = 10f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.maxStack = 999;
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddIngredient(ItemID.Grenade, 15)
                 .AddIngredient(ItemID.ShroomiteBar, 1)
               .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class ShroonadeThrown : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/Shroonade";
        public override void SetDefaults()
        {
            Projectile.damage = 75;
            Projectile.width = 15;
            Projectile.height = 20;
            Projectile.aiStyle = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 999;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.3f);
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as transparent for about 3 frames
                Projectile.alpha = 255;

                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.Resize(150, 150);

                Projectile.damage = 150;
                Projectile.knockBack = 10f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Main.rand.NextBool(4))
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 velocity = new Vector2(-6, 0).RotatedByRandom(360);
                    velocity *= 1f - Main.rand.NextFloat(0.4f);
                    Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, velocity, ModContent.ProjectileType<ShroonadeSpore>(), Projectile.damage / 2, 2, Projectile.owner);
                    int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueTorch, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                    Main.dust[dust].velocity *= 8f;
                }
            }
            if (Projectile.direction == 1)
            {
                if (Projectile.velocity.X >= 0.1f)
                {
                    // If the projectile hits the left or right side of the tile, reverse the X velocity
                    if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                    {
                        Projectile.velocity.X = -oldVelocity.X;
                    }

                    // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
                    if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                    {
                        Projectile.velocity.Y = -oldVelocity.Y;
                    }

                }
                else
                    Projectile.Kill();
            }
            if (Projectile.direction == -1)
            {
                if (-Projectile.velocity.X >= 0.1f)
                {

                    // If the projectile hits the left or right side of the tile, reverse the X velocity
                    if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                    {
                        Projectile.velocity.X = -oldVelocity.X;
                    }

                    // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
                    if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                    {
                        Projectile.velocity.Y = -oldVelocity.Y;
                    }

                }
                else
                    Projectile.Kill();
            }

            Projectile.velocity *= 0.60f;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.velocity *= -0.60f;

            for (int i = 0; i < 4; i++)
            {
                Vector2 velocity = new Vector2(-6, 0).RotatedByRandom(360);
                velocity *= 1f - Main.rand.NextFloat(0.4f);
                Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, velocity, ModContent.ProjectileType<ShroonadeSpore>(), Projectile.damage / 2, 2, Projectile.owner);
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueTorch, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                Main.dust[dust].velocity *= 8f;
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(0, -17), Vector2.Zero, ModContent.ProjectileType<ShroonadeEx>(), Projectile.damage * 2, 8, Projectile.owner);
            for (int i = 0; i < 10; i++)
            {
                Vector2 velocity = new Vector2(-9, 0).RotatedByRandom(360);
                velocity *= 1f - Main.rand.NextFloat(0.4f);
                Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, velocity, ModContent.ProjectileType<ShroonadeSpore>(), Projectile.damage / 2, 2, Projectile.owner);
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueTorch, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                Main.dust[dust].velocity *= 8f;
            }
            for (int i = 0; i < 3; i++)
            {
                Vector2 velocity = new Vector2(-5, 0).RotatedByRandom(360);
                velocity *= 1f - Main.rand.NextFloat(0.4f);
                Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, velocity, ModContent.ProjectileType<ShroonadeFX2>(), 0, 0, Projectile.owner);
            }
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShroonadeFX>(), Projectile.damage * 2, 8, Projectile.owner);

        }
    }
    public class ShroonadeEx : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 9;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 84;
            Projectile.height = 84;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 120;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
        }
        public override void AI()
        {
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.Kill();
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = lightColor;
            color.A = 0;
            return color;
        }
    }
    public class ShroonadeSpore : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 18;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
        }
        public override void AI()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 150)
                Projectile.alpha += 20;
            if (Projectile.ai[0] > 20f)
            {
                Projectile.friendly = true;
            }
            Projectile.velocity *= 0.95f;
        }
    }
    public class ShroonadeFX : ModProjectile
    {
        public override string Texture => "PenumbraMod/Assets/Textures/FireTextures/FireTexture-BitSmallBG";
        public override void SetDefaults()
        {
            Projectile.damage = 150;
            Projectile.width = 162;
            Projectile.height = 160;
            Projectile.friendly = true;
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
            return new Color(0, 0, 255, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.alpha += 5;
            if (Projectile.alpha > 255)
                Projectile.Kill();
            Projectile.rotation += 0.1f;
        }
    }
    public class ShroonadeFX2 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Assets/Textures/FireTextures/FireTexture2-BitSmallBG";
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
            return new Color(0, 0, 255, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.alpha += 5;
            Projectile.rotation += 0.1f;
        }
    }
}