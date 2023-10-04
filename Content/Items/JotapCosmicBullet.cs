using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;


namespace PenumbraMod.Content.Items
{
    public class JotapCosmicBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cosmic Bullet"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 21; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 600;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 300);
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
                    Main.EntitySpriteDraw(texture, lerpedPos, null, color * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                }
            }
            return true;
        }
        public Color color;
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(228, 87, 255, 0) * Projectile.Opacity;
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
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.RainbowRod, 0f, 0f, 0);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.8f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.011f;
            float maxDetectRadius = 300f; // The maximum radius at which a projectile can detect a target
            Projectile.ai[0] += 1f;

            // Trying to find NPC closest to the projectile
            NPC closestNPC = FindClosestNPC(maxDetectRadius);
            if (closestNPC == null)
                return;
            float rotTarget = Utils.ToRotation(closestNPC.Center - Projectile.Center);
            float rotCur = Utils.ToRotation(Projectile.velocity);
            float rotMax = MathHelper.ToRadians(5f);

            // If found, change the velocity of the projectile and turn it in the direction of the target
            Projectile.velocity = Utils.RotatedBy(Projectile.velocity, MathHelper.WrapAngle(MathHelper.WrapAngle(Utils.AngleTowards(rotCur, rotTarget, rotMax)) - Utils.ToRotation(Projectile.velocity)));
        }
       
        // Finding the closest NPC to attack within maxDetectDistance range
        // If not found then returns null
        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                // Check if NPC able to be targeted. It means that NPC is
                // 1. active (alive)
                // 2. chaseable (e.g. not a cultist archer)
                // 3. max life bigger than 5 (e.g. not a critter)
                // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                // 5. hostile (!friendly)
                // 6. not immortal (e.g. not a target dummy)
                if (target.CanBeChasedBy())
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }



        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 50; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PurpleTorch, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1.9f);
                Main.dust[dust].velocity *= 6.0f;
            }
            Vector2 launchVelocity2 = new Vector2(-10, 1); // Create a velocity moving the left.
            for (int i = 0; i < 15; i++)
            {
                // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                launchVelocity2 = launchVelocity2.RotatedBy(MathHelper.PiOver4);

                // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity2, ModContent.ProjectileType<JotapCosmicBulletMini>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item84, Projectile.position);
        }
        public class JotapCosmicBulletMini : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Cosmic Bullet"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
                ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            }

            public override void SetDefaults()
            {
                Projectile.damage = 170;
                Projectile.width = 32;
                Projectile.height = 32;
                Projectile.aiStyle = 1;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.penetrate = -1;
                Projectile.timeLeft = 600;
                Projectile.light = 0.25f;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
            }
            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                target.AddBuff(BuffID.ShadowFlame, 300);
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
                        Main.EntitySpriteDraw(texture, lerpedPos, null, color * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                    }
                }
                return true;
            }
            public Color color;
            public override Color? GetAlpha(Color lightColor)
            {
                // return Color.White;
                return new Color(255, 31, 249, 0) * Projectile.Opacity;
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
                int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.RainbowRod, 0f, 0f, 0);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.8f;
                Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.011f;

            }
        }
    }
}