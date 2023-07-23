using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Common;
using PenumbraMod.Common.Base;
using PenumbraMod.Content.Buffs;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.NPCs.Bosses.Eyestorm
{
    public class Eyeproj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Bolt"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Main.projFrames[Projectile.type] = 13;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 15;
            Projectile.width = 30;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<LowVoltage>(), 120);
        }

        public override void AI()
        {
            int dust2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueTorch, 0f, 2f, 0);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 2.6f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 135) * 0.010f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }

            if (++Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }

    }
    public class EyeprojEmpty : ModProjectile
    {
        public override string Texture => "PenumbraMod/EMPTY";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Bolt"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 20;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            int dust2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueTorch, 0f, 2f, 0);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 2.6f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 135) * 0.010f;

        }

    }
    public class EyeprojGlow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Bolt");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;// By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.damage = 15;
            Projectile.width = 40;
            Projectile.height = 23;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<LowVoltage>(), 120);
        }

        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
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
            Vector2 vel = new Vector2(0, 0);
            Projectile.ai[0] += 1;
            if (Projectile.ai[0] < 2)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, vel, ModContent.ProjectileType<Brightness10>(), 0, 0f, Main.myPlayer);
                SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D glow = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/EyeprojGlowEf").Value;
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
                    Main.EntitySpriteDraw(glow, lerpedPos, null, Color.White * 0.1f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                    Main.EntitySpriteDraw(texture, lerpedPos, null, color * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);

                }
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 225, 255, 0) * Projectile.Opacity;
        }
        public override void Kill(int timeLeft)
        {
            Vector2 vel = new Vector2(0, 0);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, vel, ModContent.ProjectileType<Brightness10>(), 0, 0f, Main.myPlayer);
            SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
        }
    }
    public class EyeprojGlow2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Bolt");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;// By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.damage = 15;
            Projectile.width = 40;
            Projectile.height = 23;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<LowVoltage>(), 120);
        }

        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
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
            Vector2 vel = new Vector2(0, 0);
            Projectile.ai[0] += 1;
            if (Projectile.ai[0] < 2)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, vel, ModContent.ProjectileType<Brightness10>(), 0, 0f, Main.myPlayer);
                SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
            }
            Projectile.velocity *= 1.03f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D glow = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/EyeprojGlowEf").Value;
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
                    Main.EntitySpriteDraw(glow, lerpedPos, null, Color.White * 0.1f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                    Main.EntitySpriteDraw(texture, lerpedPos, null, color * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);

                }
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 225, 255, 0) * Projectile.Opacity;
        }
        public override void Kill(int timeLeft)
        {
            Vector2 vel = new Vector2(0, 0);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, vel, ModContent.ProjectileType<Brightness10>(), 0, 0f, Main.myPlayer);
            SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
        }
    }
    public class Eyeproj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Bolt"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Main.projFrames[Projectile.type] = 13;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 15;
            Projectile.width = 30;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<LowVoltage>(), 120);
        }
        public override void AI()
        {
            int dust2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueTorch, 0f, 2f, 0);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 2.6f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 135) * 0.010f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
            Projectile.velocity *= 1.03f;
            if (++Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }

    }
    public class Eyeproj4 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Bolt"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Main.projFrames[Projectile.type] = 13;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 15;
            Projectile.width = 30;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 240;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<LowVoltage>(), 120);
        }
        int f;
        public override void AI()
        {
            f++;
            Player player = Main.player[Projectile.owner];

            int dust2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueTorch, 0f, 2f, 0);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 2.6f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 135) * 0.010f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
            if (++Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            if (f > 25)
            {
                if (player.velocity.X > 0.4f)
                {
                    float projSpeed = 6;
                    Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
                }
                if (player.velocity.Y > 0.4f)
                {
                    float projSpeed = 6;
                    Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
                }
            }
            if (f > 210)
            {
                Projectile.alpha += 20;
            }
        }

    }
    public class EyeprojKill : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Bolt"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 999;
            Projectile.width = 36;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 10;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.scale = 0.6f;
        }

        public override void AI()
        {
            int dust2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueTorch, 0f, 2f, 0);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 2.6f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 135) * 0.010f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }

            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

        }

    }
    public class ConductorProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Bolt");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;// By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }
        public override void SetDefaults()
        {
            Projectile.damage = 15;
            Projectile.width = 9;
            Projectile.height = 9;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<LowVoltage>(), 120);
        }

        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
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
            Vector2 vel = new Vector2(0, 0);
            Projectile.ai[0] += 1;
            if (Projectile.ai[0] < 2)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, vel, ModContent.ProjectileType<Brightness10>(), 0, 0f, Main.myPlayer);
                SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
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
                    Color finalColor = lightColor * (1 - ((float)k / (float)Projectile.oldPos.Length));
                    finalColor.A = 0;
                    Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 0);
                }
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 225, 255, 0) * Projectile.Opacity;
        }
        public override void Kill(int timeLeft)
        {
            Vector2 vel = new Vector2(0, 0);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, vel, ModContent.ProjectileType<Brightness10>(), 0, 0f, Main.myPlayer);
            SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
        }
    }
    public class Eyeproj3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Bolt"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Main.projFrames[Projectile.type] = 13;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 17;
            Projectile.width = 30;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<LowVoltage>(), 120);
        }
        public override void AI()
        {
            int dust2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueTorch, 0f, 2f, 0);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 2.6f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 135) * 0.010f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
            Projectile.velocity *= 1.08f;
            if (++Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }

    }
    public class Brightness : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 0, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.scale += 0.04f;
            Projectile.alpha -= 20;
            NPC npc = PenumbraUtils.NPCExists(Projectile.ai[1], ModContent.NPCType<Eyeofthestorm>());
            if (npc != null)
            {
                Projectile.Center = npc.Center;
            }
            else
            {
                Projectile.alpha += 30;
                if (Projectile.alpha > 255)
                {
                    Projectile.Kill();
                }

            }
        }
    }
    public class Brightness2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;

            Projectile.scale = 0.1f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 0, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.scale += 0.05f;
            Projectile.alpha += 15;


        }
    }
    public class Brightness10 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 75;
            Projectile.height = 75;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
                return new Color(0, 0, 255, 0) * Projectile.Opacity;
            else
                return new Color(255, 255, 255, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.scale += 0.05f;
            Projectile.alpha += 20;
        }
    }
    public class EXBEAM : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 70;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;

        }
        int counter;
        public override Color? GetAlpha(Color lightColor)
        {
            if (counter >= 40 && counter <= 50 || counter >= 60 && counter <= 70)
                return new Color(0, 225, 225, 0) * Projectile.Opacity;
            else
                return new Color(0, 0, 255, 0) * Projectile.Opacity;
        }
        float intensity = 0f;
        int c;
        public override void AI()
        {
            counter++;
            c++;
            const float alpha = 0.03f;

            if (c < 40)
            {
                Projectile.rotation += 0.2f;
                intensity += alpha;
                if (intensity > 1f)
                {
                    intensity = 1f;
                }
            }
            else
            {
                Projectile.rotation += 0.9f;
                intensity -= alpha;
                if (intensity < 0f)
                {
                    intensity = 0f;
                }
            }

            if (counter == 40)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Brightness6>(), 0, 0f, Main.myPlayer);
            }
            if (counter == 60)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Brightness6>(), 0, 0f, Main.myPlayer);
            }
            Projectile.velocity *= 0.96f;

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D glow = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/Brightness9").Value;
            Color color = Projectile.GetAlpha(lightColor);
            Vector2 pos = Projectile.Center + Utils.RandomVector2(Main.rand, 50, 50);
            Vector2 pos2 = Projectile.Center + Utils.RandomVector2(Main.rand, -50, -50);
            Main.EntitySpriteDraw(glow, pos - Main.screenPosition, null, color * intensity, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 2), SpriteEffects.None, 0);
            Main.EntitySpriteDraw(glow, pos2 - Main.screenPosition, null, color * intensity, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1.2f, 1.3f), SpriteEffects.None, 0);

            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueTorch, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 2.9f);
                Main.dust[dust].velocity *= 3.0f;
            }
            SoundEngine.PlaySound(SoundID.Item89, Projectile.Center);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<EXBEAMHIT>(), 10, 0f, Main.myPlayer);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Brightness6>(), 0, 0f, Main.myPlayer);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Brightness7>(), 0, 0f, Main.myPlayer);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Brightness8>(), 0, 0f, Main.myPlayer);
        }
    }
    public class EXBEAMHIT : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Explosion"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<LowVoltage>(), 120);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 225, 225, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.alpha += 30;
            Projectile.scale += 0.1f;
        }
    }
    public class Brightness3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.scale += 0.12f;
            Projectile.alpha += 10;
            Projectile.rotation += 0.03f;
            NPC npc = PenumbraUtils.NPCExists(Projectile.ai[1], ModContent.NPCType<Eyeofthestorm>());
            if (npc != null)
            {

                Projectile.Center = npc.Center;
            }
            else
            {
                Projectile.alpha += 30;
                if (Projectile.alpha > 255)
                {
                    Projectile.Kill();
                }

            }
        }
    }
    public class Brightness4 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.scale += 0.17f;
            Projectile.alpha += 10;
            Projectile.rotation += 0.07f;
            NPC npc = PenumbraUtils.NPCExists(Projectile.ai[1], ModContent.NPCType<Eyeofthestorm>());
            if (npc != null)
            {
                Projectile.Center = npc.Center;

            }
            else
            {

                Projectile.alpha += 30;
                if (Projectile.alpha > 255)
                {
                    Projectile.Kill();
                }
            }
        }
    }
    public class Brightness5 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 10;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.scale += 0.17f;
            Projectile.alpha += 10;
            Projectile.rotation += 0.13f;
            NPC npc = PenumbraUtils.NPCExists(Projectile.ai[1], ModContent.NPCType<Eyeofthestorm>());
            if (npc != null)
            {
                Projectile.Center = npc.Center;
            }
            else
            {
                Projectile.alpha += 30;
                if (Projectile.alpha > 255)
                {
                    Projectile.Kill();
                }

            }
        }
    }
    public class Brightness6 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 0, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.scale -= 0.05f;
            Projectile.alpha += 20;
            Projectile.velocity *= 0.60f;
        }
    }
    public class BrightnessP2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 100;
            Projectile.aiStyle = 0;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 0, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.scale -= 0.01f;
            Projectile.alpha += 10;
            Projectile.velocity *= 0.60f;
        }
    }
    public class BrightnessP22 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 75;
            Projectile.height = 75;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 100;
            Projectile.aiStyle = 0;
            Projectile.alpha = 255;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
                return new Color(0, 0, 255, 0) * Projectile.Opacity;
            else
                return new Color(255, 255, 255, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.scale += 0.01f;
            Projectile.alpha -= 20;
            Projectile.velocity *= 0.60f;
        }
    }
    public class Brightness9 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 100;
            Projectile.aiStyle = 0;
            Projectile.alpha = 255;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.scale -= 0.05f;
            Projectile.rotation += 1.2f;
            Projectile.alpha -= 20;
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            Projectile.velocity *= 0.60f;

        }
    }
    public class Brightness7 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 0, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.scale += 0.05f;
            Projectile.alpha += 10;
            Projectile.velocity *= 0.60f;
        }
    }
    public class Brightness8 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 75;
            Projectile.height = 75;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
                return new Color(0, 0, 255, 0) * Projectile.Opacity;
            else
                return new Color(255, 255, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.scale += 0.07f;
            Projectile.alpha += 20;
            Projectile.velocity *= 0.60f;
        }
    }
    public class Brightness2Death : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 0, 255, 0) * Projectile.Opacity;

        }

        public override void AI()
        {
            Projectile.scale += 0.05f;
            Projectile.alpha += 20;
            NPC npc = PenumbraUtils.NPCExists(Projectile.ai[1], ModContent.NPCType<Eyeofthestorm>());
            if (npc != null)
            {
                Projectile.Center = npc.Center;
            }
            else
            {
                Projectile.alpha += 30;
                if (Projectile.alpha > 255)
                {
                    Projectile.Kill();
                }
            }
        }

    }

    public class BrightnessDeath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brightness"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 70;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
                return new Color(0, 0, 255, 0) * Projectile.Opacity;
            else
                return new Color(255, 255, 255, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.scale += 0.01f;
            Projectile.alpha += 10;
        }

    }
    public class LazerBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Beam");
            Main.projFrames[Projectile.type] = 2;            
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 1900;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 40;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
        }
        public int counter;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 0, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                Projectile.Kill();
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
            Player owner = Main.player[Projectile.owner];
            counter++;
            if (counter >= 2 && counter <= 29)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }
            if (counter > 31)
            {
                Projectile.alpha += 20;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                }
                Projectile.scale += 0.03f;
                Projectile.velocity.Y -= 3f;
            }
            if (counter == 30)
            {
                Projectile.hostile = true;
                Projectile.frame = 1;
                SoundEngine.PlaySound(SoundID.Thunder, Projectile.Center);
                PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 12f, 6f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
            }
        }
    }
    public class Line5 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Beam");
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 1900;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 40;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
        }
        public int counter;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                Projectile.Kill();
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
            Player owner = Main.player[Projectile.owner];
            counter++;
            if (counter >= 2 && counter <= 29)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 120)
                {
                    Projectile.alpha = 120;
                }
            }
            if (counter > 31)
            {
                Projectile.alpha += 20;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                }
                Projectile.scale += 0.03f;
                Projectile.velocity.Y -= 0.05f;
            }
            if (counter == 30)
            {
                Projectile.frame = 1;
            }
        }
    }
    public class LazerBeam3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Laser");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 3800;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 250;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
        }
        public int counter;
        int counter2;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 0, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                Projectile.Kill();
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
            Player owner = Main.player[Projectile.owner];
            counter++;
            counter2++;
            if (counter >= 2 && counter <= 59)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }
            if (counter2 > 220)
            {
                Projectile.alpha += 10;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                }
                Projectile.scale += 0.01f;
            }
            if (counter == 60)
            {
                Projectile.scale += 0.08f;
                Projectile.hostile = true;
                Projectile.frame = 1;
                SoundEngine.PlaySound(SoundID.Thunder, Projectile.Center);
                PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 12f, 6f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
            }
            if (counter == 70)
            {
                Projectile.frame = 1;
            }
            if (counter == 75)
            {
                Projectile.frame = 2;
            }
            if (counter == 80)
            {
                Projectile.frame = 3;
                counter = 70;
            }
            if (counter2 > 70)
            {
                Projectile.rotation += 0.01f;
            }

        }
        Vector2 dir = Vector2.Zero;
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + MathHelper.Pi; // The rotation of the Jousting Lance.
            float scaleFactor = 3800f;
            float scaleFactor2 = -3800f;// How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 75f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            // This Rectangle is the width and height of the Jousting Lance's hitbox which is used for the first step of collision.
            // You will need to modify the last two numbers if you have a bigger or smaller Jousting Lance.
            // Vanilla uses (0, 0, 300, 300) which that is quite large for the size of the Jousting Lance.
            // The size doesn't matter too much because this rectangle is only a basic check for the collision (the hit-line is much more important).
            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 3800, 3800);

            // Set the position of the large rectangle.
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;
            Vector2 hitLineEnd2 = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor2;

            // The following is for debugging the size of the hit line. This will allow you to easily see where it starts and ends.
            //Dust.NewDustPerfect(Projectile.Center, DustID.Pixie, Velocity: Vector2.Zero, 1);
            Dust.NewDustPerfect(hitLineEnd, DustID.BlueTorch, Velocity: Vector2.Zero, 1);
            Dust.NewDustPerfect(hitLineEnd2, DustID.BlueTorch, Velocity: Vector2.Zero, 1);
            for (int x = 0; x < 30; x += 16)
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 hitLineBT = Projectile.Center + rotationFactor.ToRotationVector2() * -x;
                    Dust.NewDustPerfect(hitLineBT, DustID.BlueTorch, Velocity: new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)));
                }
            }
            hlende = hitLineEnd;
            // First check that our large rectangle intersects with the target hitbox.
            // Then we check to see if a line from the tip of the Jousting Lance to the "end" of the lance intersects with the target hitbox.
            if (/*lanceHitboxBounds.Intersects(targetHitbox)
                && */Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
            {
                return true;
            }
            if (/*lanceHitboxBounds.Intersects(targetHitbox)
                && */Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd2, widthMultiplier * Projectile.scale, ref collisionPoint))
            {
                return true;
            }
            return false;
        }
    }
    public class Line3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Beam");
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 3800;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 250;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
            Projectile.rotation = 95;
        }
        public int counter;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                Projectile.Kill();
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
            Player owner = Main.player[Projectile.owner];
            counter++;
            if (counter >= 2 && counter <= 59)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 120)
                {
                    Projectile.alpha = 120;
                }
            }

            if (counter == 60)
            {
                Projectile.scale += 0.08f;
                Projectile.frame = 1;
            }
            if (counter > 70)
            {
                Projectile.alpha += 10;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                }
                Projectile.rotation += 0.01f;
            }
        }
    }
    public class Line4 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Beam");
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 3800;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 250;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
        }
        public int counter;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                Projectile.Kill();
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
            Player owner = Main.player[Projectile.owner];
            counter++;
            if (counter >= 2 && counter <= 59)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 120)
                {
                    Projectile.alpha = 120;
                }
            }

            if (counter == 60)
            {
                Projectile.scale += 0.08f;
                Projectile.frame = 1;
            }
            if (counter > 70)
            {
                Projectile.alpha += 10;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                }
                Projectile.rotation += 0.01f;
            }
        }
    }
    public class LazerBeam4 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Beam");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 3800;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 250;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
            Projectile.rotation = 95;
        }
        public int counter;
        int counter2;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 0, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                Projectile.Kill();
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
            Player owner = Main.player[Projectile.owner];
            counter++;
            counter2++;
            if (counter >= 2 && counter <= 59)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }
            if (counter2 > 220)
            {
                Projectile.alpha += 10;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                }
                Projectile.scale += 0.01f;
            }
            if (counter == 60)
            {
                Projectile.scale += 0.08f;
                Projectile.hostile = true;
                Projectile.frame = 1;
                SoundEngine.PlaySound(SoundID.Thunder, Projectile.Center);
                PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 12f, 6f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
            }
            if (counter == 70)
            {
                Projectile.frame = 1;
            }
            if (counter == 75)
            {
                Projectile.frame = 2;
            }
            if (counter == 80)
            {
                Projectile.frame = 3;
                counter = 70;
            }
            if (counter2 > 70)
            {
                Projectile.rotation += 0.01f;
            }

        }
        Vector2 dir = Vector2.Zero;
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + MathHelper.Pi; // The rotation of the Jousting Lance.
            float scaleFactor = 3800f;
            float scaleFactor2 = -3800f;// How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 75f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            // This Rectangle is the width and height of the Jousting Lance's hitbox which is used for the first step of collision.
            // You will need to modify the last two numbers if you have a bigger or smaller Jousting Lance.
            // Vanilla uses (0, 0, 300, 300) which that is quite large for the size of the Jousting Lance.
            // The size doesn't matter too much because this rectangle is only a basic check for the collision (the hit-line is much more important).
            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 3800, 3800);

            // Set the position of the large rectangle.
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;
            Vector2 hitLineEnd2 = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor2;

            // The following is for debugging the size of the hit line. This will allow you to easily see where it starts and ends.
            //Dust.NewDustPerfect(Projectile.Center, DustID.Pixie, Velocity: Vector2.Zero, 1);
            Dust.NewDustPerfect(hitLineEnd, DustID.BlueTorch, Velocity: Vector2.Zero, 1);
            Dust.NewDustPerfect(hitLineEnd2, DustID.BlueTorch, Velocity: Vector2.Zero, 1);
            for (int x = 0; x < 30; x += 16)
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 hitLineBT = Projectile.Center + rotationFactor.ToRotationVector2() * -x;
                    Dust.NewDustPerfect(hitLineBT, DustID.BlueTorch, Velocity: new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)));
                }
            }
            hlende = hitLineEnd;
            // First check that our large rectangle intersects with the target hitbox.
            // Then we check to see if a line from the tip of the Jousting Lance to the "end" of the lance intersects with the target hitbox.
            if (/*lanceHitboxBounds.Intersects(targetHitbox)
                && */Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
            {
                return true;
            }
            if (/*lanceHitboxBounds.Intersects(targetHitbox)
                && */Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd2, widthMultiplier * Projectile.scale, ref collisionPoint))
            {
                return true;
            }
            return false;
        }
    }

    public class LazerBeam2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.width = 50;
            Projectile.height = 1900;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.alpha = 90;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 0, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.alpha += 10;
            Projectile.scale += 0.05f;
            Projectile.velocity.Y -= 3f;
            SoundEngine.PlaySound(SoundID.Thunder, Projectile.Center);
            PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 18f, 8f, 30, 1000f, FullName);
            Main.instance.CameraModifiers.Add(modifier);
        }
    }

    public class LightningA : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lightning Beam");
            // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.width = 9;
            Projectile.height = 1498;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 80;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.alpha = 180;
        }
        int i;
        public override Color? GetAlpha(Color lightColor)
        {
            if (i < 40)
                return new Color(0, 0, 255, 0) * Projectile.Opacity;
            else
                return new Color(0, 225, 225, 0) * Projectile.Opacity;
        }

        public override void AI()
        {
            i++;
            if (!NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                Projectile.Kill();
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
            if (i == 40)
            {
                Projectile.hostile = true;
                Projectile.alpha = 0;
                if (Projectile.alpha > 1)
                {
                    Projectile.alpha = 0;
                }
                SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
            }
            if (i > 41)
            {
                Projectile.alpha += 10;
                Projectile.hostile = true;
            }
            Projectile.scale += 0.05f;

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/LightningLine").Value;
            if (!Projectile.hostile)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
            }
            return true;
        }
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + MathHelper.PiOver2; // The rotation of the Jousting Lance.
            float scaleFactor = 1498f;
            float scaleFactor2 = -1498f;// How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 10f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 1498, 1498);
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;
            Vector2 hitLineEnd2 = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor2;
            Dust.NewDustPerfect(hitLineEnd, DustID.BlueTorch, Velocity: Vector2.Zero, 1);
            Dust.NewDustPerfect(hitLineEnd2, DustID.BlueTorch, Velocity: Vector2.Zero, 1);

            hlende = hitLineEnd;
            if (Projectile.alpha < 100)
            {
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd2, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
            }

            return false;
        }
    }
    public class LightningB : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lightning Beam");
            // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.width = 9;
            Projectile.height = 1498;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 80;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.rotation = 10;
            Projectile.alpha = 180;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (i < 40)
                return new Color(0, 0, 255, 0) * Projectile.Opacity;
            else
                return new Color(0, 225, 225, 0) * Projectile.Opacity;
        }
        int i;
        public override void AI()
        {
            i++;
            if (!NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                Projectile.Kill();
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
            if (i == 40)
            {
                Projectile.hostile = true;
                Projectile.alpha = 0;
                if (Projectile.alpha > 1)
                {
                    Projectile.alpha = 0;
                }
                SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
            }
            if (i > 41)
            {
                Projectile.alpha += 10;
            }
            Projectile.scale += 0.05f;

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/LightningLine").Value;
            if (!Projectile.hostile)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
            }
            return true;
        }
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + MathHelper.PiOver2; // The rotation of the Jousting Lance.
            float scaleFactor = 1498f;
            float scaleFactor2 = -1498f;// How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 10f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 1498, 1498);
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;
            Vector2 hitLineEnd2 = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor2;
            Dust.NewDustPerfect(hitLineEnd, DustID.BlueTorch, Velocity: Vector2.Zero, 1);
            Dust.NewDustPerfect(hitLineEnd2, DustID.BlueTorch, Velocity: Vector2.Zero, 1);

            hlende = hitLineEnd;
            if (Projectile.alpha < 100)
            {
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd2, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
            }

            return false;
        }
    }
    public class LightningC : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lightning Beam");
            // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.width = 9;
            Projectile.height = 1498;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 80;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.rotation = -10;
            Projectile.alpha = 180;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (i < 40)
                return new Color(0, 0, 255, 0) * Projectile.Opacity;
            else
                return new Color(0, 225, 225, 0) * Projectile.Opacity;
        }
        int i;
        public override void AI()
        {
            i++;
            if (!NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                Projectile.Kill();
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
            if (i == 40)
            {
                Projectile.hostile = true;
                Projectile.alpha = 0;
                if (Projectile.alpha > 1)
                {
                    Projectile.alpha = 0;
                }
            }
            if (i > 41)
            {
                Projectile.alpha += 10;
            }
            Projectile.scale += 0.05f;

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/LightningLine").Value;
            if (!Projectile.hostile)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
            }
            return true;
        }
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + MathHelper.PiOver2; // The rotation of the Jousting Lance.
            float scaleFactor = 1498f;
            float scaleFactor2 = -1498f;// How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 10f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 1498, 1498);
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;
            Vector2 hitLineEnd2 = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor2;
            Dust.NewDustPerfect(hitLineEnd, DustID.BlueTorch, Velocity: Vector2.Zero, 1);
            Dust.NewDustPerfect(hitLineEnd2, DustID.BlueTorch, Velocity: Vector2.Zero, 1);

            hlende = hitLineEnd;
            if (Projectile.alpha < 100)
            {
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd2, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
            }

            return false;
        }
    }
    public class LightningD : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lightning Beam");
            // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.width = 9;
            Projectile.height = 1498;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 80;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.rotation = -15;
            Projectile.alpha = 180;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (i < 40)
                return new Color(0, 0, 255, 0) * Projectile.Opacity;
            else
                return new Color(0, 225, 225, 0) * Projectile.Opacity;
        }
        int i;
        public override void AI()
        {
            i++;
            if (!NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                Projectile.Kill();
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
            if (i == 40)
            {
                Projectile.hostile = true;
                Projectile.alpha = 0;
                if (Projectile.alpha > 1)
                {
                    Projectile.alpha = 0;
                }
            }
            if (i > 41)
            {
                Projectile.alpha += 10;
            }
            Projectile.scale += 0.05f;

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/LightningLine").Value;
            if (!Projectile.hostile)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
            }
            return true;
        }
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + MathHelper.PiOver2; // The rotation of the Jousting Lance.
            float scaleFactor = 1498f;
            float scaleFactor2 = -1498f;// How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 10f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 1498, 1498);
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;
            Vector2 hitLineEnd2 = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor2;
            Dust.NewDustPerfect(hitLineEnd, DustID.BlueTorch, Velocity: Vector2.Zero, 1);
            Dust.NewDustPerfect(hitLineEnd2, DustID.BlueTorch, Velocity: Vector2.Zero, 1);

            hlende = hitLineEnd;
            if (Projectile.alpha < 100)
            {
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd2, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
            }

            return false;
        }
    }
    public class LightningE : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lightning Beam");
            // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.width = 9;
            Projectile.height = 1498;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 80;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.rotation = 15;
            Projectile.alpha = 180;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (i < 40)
                return new Color(0, 0, 255, 0) * Projectile.Opacity;
            else
                return new Color(0, 225, 225, 0) * Projectile.Opacity;
        }
        int i;
        public override void AI()
        {
            i++;
            if (!NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                Projectile.Kill();
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
            if (i == 40)
            {
                Projectile.hostile = true;
                Projectile.alpha = 0;
                if (Projectile.alpha > 1)
                {
                    Projectile.alpha = 0;
                }
                PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 8f, 6f, 30, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
            }
            if (i > 41)
            {
                Projectile.alpha += 10;
            }
            Projectile.scale += 0.05f;

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/LightningLine").Value;
            if (!Projectile.hostile)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
            }
            return true;
        }
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + MathHelper.PiOver2; // The rotation of the Jousting Lance.
            float scaleFactor = 1498f;
            float scaleFactor2 = -1498f;// How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 10f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 1498, 1498);
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;
            Vector2 hitLineEnd2 = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor2;
            Dust.NewDustPerfect(hitLineEnd, DustID.BlueTorch, Velocity: Vector2.Zero, 1);
            Dust.NewDustPerfect(hitLineEnd2, DustID.BlueTorch, Velocity: Vector2.Zero, 1);

            hlende = hitLineEnd;
            if (Projectile.alpha < 100)
            {
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd2, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
            }

            return false;
        }
    }
    public class Line6 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Storm Beam");
            // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 1900;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.alpha = 120;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.alpha += 20;
            Projectile.scale += 0.05f;
            if (!NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                Projectile.Kill();
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
        }
    }
    public class Warning : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Beams Warning");
            // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 35;
            Projectile.height = 1894;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 50;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 223, 255, 0) * Projectile.Opacity;

        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 40)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }
            if (Projectile.ai[0] > 41)
            {
                Projectile.alpha += 20;
            }
            Projectile.scale += 0.05f;
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
        }
    }

    public class Arrows : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Arrows"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 600;
            Projectile.height = 64;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 270;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            return true;
        }
        double deg;
        int t;
        public override void AI()
        {
            t++;
            if (t < 30)
            {
                Projectile.alpha -= 20;
            }
            if (Main.npc[PenumbraGlobalNPC.eyeStorm].life < 10)
            {
                Projectile.Kill();
            }
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            deg = Projectile.ai[1];
            double rad = deg * (Math.PI / 180);
            double dist = 15;

            Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
            if (t > 240)
            {
                Projectile.alpha += 30;
            }

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }
    }
    public class ExplosionDeath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Explosion"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 98;
            Projectile.height = 98;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            return true;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueTorch, 0f, 0f, 0);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 3f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 170) * 0.012f;

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

        }
    }
    public class LoopSoundProj : ModProjectile
    {
        public override string Texture => "PenumbraMod/EMPTY";
        internal enum ActiveSoundShowcaseStyle
        {
            LoopedSoundAdvanced,
            n,
        }
        private ActiveSoundShowcaseStyle Style
        {
            get => (ActiveSoundShowcaseStyle)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }
        SlotId soundSlot;
        SoundStyle soundStyleIgniteLoop = new SoundStyle("PenumbraMod/Assets/Sounds/SFX/ShieldAmbience")
        {
            IsLooped = true,
            SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest
        };
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.timeLeft = 2;
            NPC npc = PenumbraUtils.NPCExists(Projectile.ai[1], ModContent.NPCType<EnergyConductorMinion>());
            Projectile.Center = npc.Center;
            if (npc.life <= 0)
            {
                Projectile.Kill();
            }
            switch (Style)
            {
                case ActiveSoundShowcaseStyle.LoopedSoundAdvanced:
                    if (!SoundEngine.TryGetActiveSound(soundSlot, out var _))
                    {
                        var tracker = new ProjectileAudioTracker(Projectile);
                        soundSlot = SoundEngine.PlaySound(soundStyleIgniteLoop, Projectile.position, soundInstance =>
                        {
                            soundInstance.Position = Projectile.position;
                            return tracker.IsActiveAndInGame();
                        });
                    }
                    break;
                case ActiveSoundShowcaseStyle.n:
                    goto case ActiveSoundShowcaseStyle.LoopedSoundAdvanced;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Style == ActiveSoundShowcaseStyle.LoopedSoundAdvanced)
            {
                if (SoundEngine.TryGetActiveSound(soundSlot, out var activeSound))
                {
                    activeSound.Stop();
                }
            }
        }
    }
}