using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.NPCs.Bosses.Eyestorm;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class PrismaGlobe : ModItem
	{
		public override void SetStaticDefaults()
		{
            /* Tooltip.SetDefault("Some of the gods most used weapon" +
                "\nFires a huge lightning that inflicts high voltage on enemies"); */
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 78;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            Item.width = 28;
            Item.height = 34;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = 35000;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 16f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(25));
            // Decrease velocity randomly for nicer visuals.
            newVelocity *= 1f - Main.rand.NextFloat(0.2f);
            Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<PrismaGlobeProj>(), Item.damage, 0f, player.whoAmI);
            Projectile.NewProjectileDirect(source, position, Vector2.Zero, ModContent.ProjectileType<Brightness10>(), 0, 0f, player.whoAmI);
            if (Main.dayTime)
            {
                Item.shootSpeed = 18f;
                Item.useTime = 8;
                Item.useAnimation = 8;
            }
            else
            {
                Item.shootSpeed = 16f;
                Item.useTime = 10;
                Item.useAnimation = 10;
            }
               
            return false; // return false to stop vanilla from calling Projectile.NewProjectile.
        }    
    }
    public class PrismaGlobeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Stars Caller Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 78;
            Projectile.knockBack = 6f;
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.light = 1f;
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
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/PrismaGlobeProjef").Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
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
                    Color finalColor = Main.DiscoColor * 0.6f * (1 - ((float)k / (float)Projectile.oldPos.Length));
                    finalColor.A = 0;//acts like additive blending without spritebatch stuff
                    Main.EntitySpriteDraw(texture, lerpedPos, null, finalColor, lerpedAngle, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                }
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * 0.4f * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            Color color2 = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, color2, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
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
            float maxDetectRadius = 600f;
            Projectile.ai[0] += 1f;
            NPC closestNPC = FindClosestNPC(maxDetectRadius);
            if (closestNPC == null)
                return;
            float rotTarget = Utils.ToRotation(closestNPC.Center - Projectile.Center);
            float rotCur = Utils.ToRotation(Projectile.velocity);
            float rotMax = MathHelper.ToRadians(5f);

            // If found, change the velocity of the projectile and turn it in the direction of the target
            // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
            if (Projectile.ai[0] > 20f)
            {
                Projectile.velocity = Utils.RotatedBy(Projectile.velocity, MathHelper.WrapAngle(MathHelper.WrapAngle(Utils.AngleTowards(rotCur, rotTarget, rotMax)) - Utils.ToRotation(Projectile.velocity)));
            }
        }
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
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PinkFairy, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                Main.dust[dust].velocity *= 3f;
            }
        }
    }
}