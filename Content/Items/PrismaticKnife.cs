using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using PenumbraMod.Common.Systems;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Consumables;
using PenumbraMod.Content.Items.PrismaticScythe;
using PenumbraMod.Content.NPCs.Bosses.Eyestorm;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class PrismaticKnife : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 75;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 18;
			Item.height = 34;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.value = 23000;
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PrismaticKnifeProj>();
            Item.shootSpeed = 2f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
		}
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<PrismaticKnifeProj>()] < 8;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                Item.useStyle = 1;
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.

                // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                Projectile.NewProjectile(source, position, launchVelocity, ModContent.ProjectileType<PrismaticKnifeProjSpecial>(), 220, knockback, player.whoAmI);

            }
            else
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PrismaticKnifeProj>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
    public class PrismaticKnifeProj : ModProjectile
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
            Projectile.damage = 75;
            Projectile.knockBack = 6f;
            Projectile.width = 18;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.light = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
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
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/PrismaticKnifeef").Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/PrismaticKnifeef2").Value;
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
                    Color finalColor = Color.White * 0.3f * (1 - ((float)i / (float)Projectile.oldPos.Length));
                    finalColor.A = 0;//acts like additive blending without spritebatch stuff
                    Color finalColor2 = Color.White * (1 - ((float)i / (float)Projectile.oldPos.Length));
                    finalColor2.A = 0;//acts like additive blending without spritebatch stuff
                    if (Projectile.ai[0] > 20f)
                    {
                        Main.EntitySpriteDraw(texture, lerpedPos, null, Main.DiscoColor * 0.5f *(1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(proj.Width / 2, proj.Height / 2), Projectile.scale, SpriteEffects.None, 0);
                        Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, new Vector2(proj.Width / 2, proj.Height / 2), 0.8f, SpriteEffects.None, 0);
                        Main.EntitySpriteDraw(texture2, lerpedPos, null, finalColor2, lerpedAngle, new Vector2(proj.Width / 2, proj.Height / 2), Projectile.scale, SpriteEffects.None, 0);
                    }
                }
            }
            Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        int radius1 = 30;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.timeLeft = 2;
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
            Player player = Main.player[Projectile.owner];
            Projectile.ai[0] += 1f;
            Projectile.ai[1] += 1f;
            Projectile.ai[2] += 1f;
            float rotTarget = Utils.ToRotation(Main.MouseWorld - Projectile.Center);
            float rotCur = Utils.ToRotation(Projectile.velocity);
            float rotMax = MathHelper.ToRadians(12f);
            if (player.ownedProjectileCounts[Type] >= 8 && !player.channel)
                player.SetDummyItemTime(2);
            // If found, change the velocity of the projectile and turn it in the direction of the target
            // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
            if (Main.myPlayer == Projectile.owner)
            {
                if (Projectile.ai[0] > 20f)
                {
                    if (player.channel)
                    {
                        if (Projectile.ai[2] > 20)
                            Projectile.velocity = Utils.RotatedBy(Projectile.velocity, MathHelper.WrapAngle(MathHelper.WrapAngle(Utils.AngleTowards(rotCur, rotTarget, rotMax)) - Utils.ToRotation(Projectile.velocity)));
                        if (Projectile.ai[2] == 40)
                            Projectile.ai[2] = 11;
                        if (player.ownedProjectileCounts[Type] >= 8)
                        {
                            Projectile.extraUpdates = 2;
                        }
                        else
                            Projectile.extraUpdates = 1;
                    }
                    else
                    {

                        Projectile.velocity = Utils.RotatedBy(Projectile.velocity, MathHelper.WrapAngle(MathHelper.WrapAngle(Utils.AngleTowards(rotCur, Utils.ToRotation(player.Center - Projectile.Center), rotMax)) - Utils.ToRotation(Projectile.velocity)));
                        if (Projectile.Distance(player.Center) <= 50)
                        {
                            Projectile.Kill();
                            return;
                        }
                    }

                }
                else
                    Projectile.velocity = new Vector2(0, -10);
                if (Projectile.ai[1] == 20f)
                {
                    SoundEngine.PlaySound(SoundID.Item4, Projectile.Center);
                    const int Repeats = 20;
                    for (int i = 0; i < Repeats; ++i)
                    {
                        Vector2 position2 = Projectile.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                        int c = Dust.NewDust(position2, 1, 1, DustID.PinkFairy, 0f, 0f, 0, default(Color), 1f);
                        Main.dust[c].noGravity = true;
                        Main.dust[c].velocity *= 3f;
                        Main.dust[c].rotation += 1.1f;
                    }
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PinkFairy, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                Main.dust[dust].velocity *= 3f;
            }
        }
    }
    public class PrismaticKnifeProjSpecial : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/PrismaticKnifeProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Stars Caller Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public int PositionIndex
        {
            get => (int)Projectile.ai[1] - 1;
            set => Projectile.ai[1] = value + 1;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 75;
            Projectile.knockBack = 6f;
            Projectile.width = 18;
            Projectile.height = 34;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.light = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            Projectile.netImportant = true;
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
        bool active = false;
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/PrismaticKnifeef").Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/PrismaticKnifeef2").Value;
            if (!active)
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
                    Color finalColor = Color.White * 0.3f * (1 - ((float)i / (float)Projectile.oldPos.Length));
                    finalColor.A = 0;//acts like additive blending without spritebatch stuff
                    Color finalColor2 = Color.White * (1 - ((float)i / (float)Projectile.oldPos.Length));
                    finalColor2.A = 0;//acts like additive blending without spritebatch stuff
                    if (active)
                    {
                        Main.EntitySpriteDraw(texture, lerpedPos, null, Main.DiscoColor * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(proj.Width / 2, proj.Height / 2), Projectile.scale, SpriteEffects.None, 0);
                        Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, new Vector2(proj.Width / 2, proj.Height / 2), 0.8f, SpriteEffects.None, 0);
                        Main.EntitySpriteDraw(texture2, lerpedPos, null, finalColor2, lerpedAngle, new Vector2(proj.Width / 2, proj.Height / 2), Projectile.scale, SpriteEffects.None, 0);
                    }
                }
            }
            if (active)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        int radius1 = 30;
        int timer;
        public ref float RotationTimer => ref Projectile.ai[2];
        int mouseY;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(mouseY);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            mouseY = reader.ReadInt32();
        }
        public override void AI()
        {
            timer++;
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
            Player player = Main.player[Projectile.owner];
            const float rotation = 0.1f;
            if (!player.active && player.dead)
                Projectile.Kill();
            if (!ReaperClassSystem.ReaperClassKeybind.JustPressed && player.active && !active)
            {
                if (timer == 15 || timer == 25 || timer == 35 || timer == 45 || timer == 55 || timer == 65 || timer == 75)
                {
                    Vector2 vel = Projectile.velocity;
                    vel.RotatedBy(MathHelper.PiOver4);
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), player.Center, vel, ModContent.ProjectileType<PrismaticKnifeProjSpecial2>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                }
                player.SetDummyItemTime(2);
                Projectile.timeLeft = 600;

                float rad = (float)PositionIndex / 1 * MathHelper.PiOver4;

                if (timer < 70)
                RotationTimer += 5f;
                else
                    RotationTimer += 1f;
                float continuousRotation = MathHelper.ToRadians(RotationTimer);
                rad += continuousRotation;
                if (rad > MathHelper.PiOver4)
                {
                    rad -= MathHelper.PiOver4;
                }
                else if (rad < 0)
                {
                    rad += MathHelper.PiOver4;
                }

                float distanceFromBody = player.width + Projectile.width + 60;

                // offset is now a vector that will determine the position of the NPC based on its index
                Vector2 offset = Vector2.One.RotatedBy(rad) * distanceFromBody;

                Vector2 destination = player.Center + offset;
                Vector2 toDestination = destination - Projectile.Center;
                Vector2 toDestinationNormalized = toDestination.SafeNormalize(Vector2.Zero);

                float speed = 20f;
                float inertia = 4;

                Vector2 moveTo = toDestinationNormalized * speed;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + moveTo) / inertia;
                Projectile.rotation = Projectile.rotation.AngleLerp((new Vector2(Projectile.ai[0], mouseY) - Projectile.Center).ToRotation(), rotation);

                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.ai[0] = Main.MouseWorld.X;
                    mouseY = (int)Main.MouseWorld.Y;
                }
            }
            if (ReaperClassSystem.ReaperClassKeybind.JustPressed 
                && player.active 
                && !active 
                && timer > 70 
                && player.ownedProjectileCounts[ModContent.ProjectileType<PrismaticKnifeProjSpecial2>()] > 6)
            {
                Projectile.friendly = true;
                Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * 26f;
                Projectile.rotation = Projectile.velocity.ToRotation();
                SoundEngine.PlaySound(SoundID.Item4, Projectile.Center);
                const int Repeats = 10;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position2 = Projectile.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int c = Dust.NewDust(position2, 1, 1, DustID.PinkFairy, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[c].noGravity = true;
                    Main.dust[c].velocity *= 3f;
                    Main.dust[c].rotation += 1.1f;
                }
                active = true;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PinkFairy, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                Main.dust[dust].velocity *= 3f;
            }
        }
    }
    public class PrismaticKnifeProjSpecial2 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/PrismaticKnifeProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Stars Caller Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public int PositionIndex
        {
            get => (int)Projectile.ai[1] - 1;
            set => Projectile.ai[1] = value + 1;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 75;
            Projectile.knockBack = 6f;
            Projectile.width = 18;
            Projectile.height = 34;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.light = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            Projectile.netImportant = true;
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
        bool active = false;
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/PrismaticKnifeef").Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/PrismaticKnifeef2").Value;
            if (!active)
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
                    Color finalColor = Color.White * 0.3f * (1 - ((float)i / (float)Projectile.oldPos.Length));
                    finalColor.A = 0;//acts like additive blending without spritebatch stuff
                    Color finalColor2 = Color.White * (1 - ((float)i / (float)Projectile.oldPos.Length));
                    finalColor2.A = 0;//acts like additive blending without spritebatch stuff
                    if (active)
                    {
                        Main.EntitySpriteDraw(texture, lerpedPos, null, Main.DiscoColor * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(proj.Width / 2, proj.Height / 2), Projectile.scale, SpriteEffects.None, 0);
                        Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, new Vector2(proj.Width / 2, proj.Height / 2), 0.8f, SpriteEffects.None, 0);
                        Main.EntitySpriteDraw(texture2, lerpedPos, null, finalColor2, lerpedAngle, new Vector2(proj.Width / 2, proj.Height / 2), Projectile.scale, SpriteEffects.None, 0);
                    }
                }
            }
            if (active)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        int radius1 = 30;
        int timer;
        public ref float RotationTimer => ref Projectile.ai[2];
        int mouseY;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(mouseY);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            mouseY = reader.ReadInt32();
        }
        public override void AI()
        {
            timer++;
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
            Player player = Main.player[Projectile.owner];
            const float rotation = 0.1f;
            if (!player.active && player.dead)
                Projectile.Kill();
            if (!ReaperClassSystem.ReaperClassKeybind.JustPressed && player.active && !active)
            {
                player.SetDummyItemTime(2);
                Projectile.timeLeft = 600;

                float rad = (float)PositionIndex / 1 * MathHelper.PiOver4;

                if (timer < 70)
                    RotationTimer += 5f;
                else
                    RotationTimer += 1f;
                float continuousRotation = MathHelper.ToRadians(RotationTimer);
                rad += continuousRotation;
                if (rad > MathHelper.PiOver4)
                {
                    rad -= MathHelper.PiOver4;
                }
                else if (rad < 0)
                {
                    rad += MathHelper.PiOver4;
                }

                float distanceFromBody = player.width + Projectile.width + 60;

                // offset is now a vector that will determine the position of the NPC based on its index
                Vector2 offset = Vector2.One.RotatedBy(rad) * distanceFromBody;

                Vector2 destination = player.Center + offset;
                Vector2 toDestination = destination - Projectile.Center;
                Vector2 toDestinationNormalized = toDestination.SafeNormalize(Vector2.Zero);

                float speed = 20f;
                float inertia = 4;

                Vector2 moveTo = toDestinationNormalized * speed;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + moveTo) / inertia;
                Projectile.rotation = Projectile.rotation.AngleLerp((new Vector2(Projectile.ai[0], mouseY) - Projectile.Center).ToRotation(), rotation);

                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.ai[0] = Main.MouseWorld.X;
                    mouseY = (int)Main.MouseWorld.Y;
                }

            }
            if (ReaperClassSystem.ReaperClassKeybind.JustPressed
                && player.active
                && !active
                && timer > 70
                && player.ownedProjectileCounts[ModContent.ProjectileType<PrismaticKnifeProjSpecial2>()] > 6)
            {
                Projectile.friendly = true;
                Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * 26f;
                Projectile.rotation = Projectile.velocity.ToRotation();
                SoundEngine.PlaySound(SoundID.Item4, Projectile.Center);
                const int Repeats = 10;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position2 = Projectile.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int c = Dust.NewDust(position2, 1, 1, DustID.PinkFairy, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[c].noGravity = true;
                    Main.dust[c].velocity *= 3f;
                    Main.dust[c].rotation += 1.1f;
                }
                active = true;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PinkFairy, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                Main.dust[dust].velocity *= 3f;
            }
        }
    }
}