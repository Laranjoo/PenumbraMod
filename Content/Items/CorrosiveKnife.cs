using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace PenumbraMod.Content.Items
{
    public class CorrosiveKnife : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 36;
            Item.height = 38;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4;
            Item.value = 42500;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CorrosiveKnifeProj>();
            Item.shootSpeed = 18f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useTurn = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CorrosiveShard>(), 10)
                 .AddIngredient(ModContent.ItemType<CorrodedPlating>(), 8)

               .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class CorrosiveKnifeProj : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/CorrosiveKnife";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

        }
        public override void SetDefaults()
        {
            Projectile.damage = 75;
            Projectile.width = 34;
            Projectile.height = 38;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.light = 1f;
            Projectile.netImportant = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        float alpha = 0f; 
        int radius1 = 30;
        int t;
        bool checkGettingBack = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);
            Projectile.ai[0] += 1f;
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 0.5f);
            if (Projectile.ai[0] > 60)
            {
                t++;
                if (t < 2)
                {
                    const int Repeats = 50;
                    for (int i = 0; i < Repeats; ++i)
                    {
                        Vector2 position2 = Projectile.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                        int c = Dust.NewDust(position2, 1, 1, DustID.GreenTorch, 0f, 0f, 0, default(Color), 1f);
                        Main.dust[c].noGravity = true;
                        Main.dust[c].velocity *= 3f;
                        Main.dust[c].rotation += 1.1f;
                    }
                }
                Projectile.penetrate = 9;
                Projectile.netUpdate = true;
                Projectile.aiStyle = 0;
                Projectile.extraUpdates = 1;
                alpha += 0.05f;
                if (alpha >= 1f)
                    alpha = 1f;
                checkGettingBack = true;
                Projectile.tileCollide = false;
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
            int r = Dust.NewDust(Projectile.Center, 1, 1, DustID.GreenTorch, 0f, 0f, 0, default(Color), 1f);
            Main.dust[r].noGravity = true;
            Main.dust[r].velocity *= 2.9f;
            Main.dust[r].rotation += 1.1f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;
        } 
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/CorrosiveKnife").Value;
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/CorrosiveKnifeef").Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/CorrosiveKnifeef2").Value;
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
                    Color finalColor = lightColor * (1 - ((float)k / (float)Projectile.oldPos.Length));
                    finalColor.A = 0;//acts like additive blending without spritebatch stuff
                    Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, texture.Size() / 2, 1, SpriteEffects.None, 0);
                    Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, proj.Size() / 2, 1, SpriteEffects.None, 0);
                    if (checkGettingBack)
                        Main.EntitySpriteDraw(texture2, lerpedPos, null, finalColor * alpha, lerpedAngle, texture2.Size() / 2, 1.1f, SpriteEffects.None, 0);
                }
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(ModContent.BuffType<Corrosion>(), 120);
        }
        public override void Kill(int timeLeft)
        {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.

            const int Repeats = 30;
            for (int i = 0; i < Repeats; ++i)
            {
                Vector2 position2 = Projectile.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                int r = Dust.NewDust(position2, 1, 1, DustID.GreenTorch, 0f, 0f, 0, default(Color), 1f);
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity *= 6.9f;
                Main.dust[r].rotation += 1.1f;
            }
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            if (!checkGettingBack)
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    } 
}