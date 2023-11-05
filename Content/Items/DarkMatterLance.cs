using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class DarkMatterLance : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dark Matter Spear");
            /* Tooltip.SetDefault("This dark spear is terrifying powerful" +
                "\nThrows a very fast lance that explodes on enemy hit, creating dark matter spheres, inflicting dark matter debuff on them" +
                "\n'Dark entities are watching...'"); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(3, 10));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 440;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 102;
            Item.height = 102;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 42500;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DarkMatterLanceProj>();
            Item.shootSpeed = 18f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DarkMatterItem>(), 25)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
    public class DarkMatterLanceProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dark Matter Spear"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Main.projFrames[Projectile.type] = 10;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

        }

        public override void SetDefaults()
        {
            Projectile.damage = 435;
            Projectile.width = 102;
            Projectile.height = 102;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.hide = false;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DarkMatter>(), 140);
        }
        int f;
        public override void AI()
        {
            f++;
            Projectile.ai[0] += 1f;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            int dust = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<DarkMatter2>(), 0f, 0f, 0);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 4f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 170) * 0.014f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;
            Lighting.AddLight(Projectile.Center, Color.Black.ToVector3() * 0.78f);

        }
        Vector2 hlende = Vector2.Zero;

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            for (int k = 0; k < 40; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<DarkMatter2>(), Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1.6f);
                Main.dust[dust].velocity *= 6.0f;
            }

            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item70, Projectile.position);
        }

        public override bool PreDraw(ref Color lightColor)
        {

            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            // Calculating frameHeight and current Y pos dependence of frame
            // If texture without animation frameHeight is always texture.Height and startY is always 0
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            // Get this frame on texture
            Rectangle sourceRectangle = new(0, startY, texture.Width, frameHeight);

            // Alternatively, you can skip defining frameHeight and startY and use this:


            Vector2 origin = sourceRectangle.Size() / 2f;

            // If image isn't centered or symmetrical you can specify origin of the sprite
            // (0,0) for the upper-left corner
            // pohhhop´hetr0ijhwet0ijwe0uijvyreqwm00u´mvwy0u´mvwy4-m08vwy40m,8wvtr,08jmbwtwbj,0imbbjm,pbwhmj,pbwhwbjmwhtj,mpbhwt,jbht,pjebt,p
            float offsetX = 20f;
            origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;

            // If sprite is vertical
            // float offsetY = 20f;
            //  origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);

            Vector2 drawOrigin = new(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, sourceRectangle, color, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects, 0);
            }

            return false;
        }



        public override void OnKill(int timeLeft)
        {

            for (int k = 0; k < 50; k++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<DarkMatter2>(), 0f, 0f, 0);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 6f;
                Main.dust[dust].scale = (float)Main.rand.Next(100, 170) * 0.014f;
            }
            Vector2 launchVelocity = new Vector2(-12, 0); // Create a velocity moving the left.
            for (int i = 0; i < 2; i++)
            {
                // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<DarkSpheres>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner, Projectile.scale = 1.0f);
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item109, Projectile.position);
        }
    }
    public class DarkSpheres : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dark Spheres"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.hide = false;
            Projectile.alpha = 0;
            Projectile.timeLeft = 240;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DarkMatter>(), 60);
            Projectile.Kill();
        }
        public override void AI()
        {
            Projectile.velocity *= 0.95f;
            Projectile.ai[0] += 1f;
            float maxDetectRadius = 400f; // The maximum radius at which a projectile can detect a target
            float projSpeed = 22f; // The speed at which the projectile moves towards the target

            int dust = Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<DarkMatter2>(), 0f, 0f, 0);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 1.8f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.009f;

            // Trying to find NPC closest to the projectile
            NPC closestNPC = FindClosestNPC(maxDetectRadius);
            if (closestNPC == null)
                return;
            if (Projectile.ai[0] >= 1 && Projectile.ai[0] <= 29)
            {
                Projectile.rotation = Projectile.AngleTo(closestNPC.Center);
                double deg = (double)Projectile.ai[1] * 14;
                double rad = deg * (Math.PI / 180);
                double dist = 150;
                Projectile.position.X = closestNPC.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
                Projectile.position.Y = closestNPC.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
                Projectile.ai[1] += 1f;
            }

            // If found, change the velocity of the projectile and turn it in the direction of the target
            // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
            if (Projectile.ai[0] > 30f)
            {
                Projectile.friendly = true;
                Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
            }


            Lighting.AddLight(Projectile.Center, Color.Black.ToVector3() * 0.78f);

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
        Vector2 hlende = Vector2.Zero;
        public static float EaseIn(float t)
        {
            return t * t;
        }

        public static float Flip(float x)
        {
            return 1 - x;
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
                    Main.EntitySpriteDraw(texture, lerpedPos, null, Color.Black * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                }
            }
            return true;
        }
        public override void OnKill(int timeLeft)
        {

            for (int k = 0; k < 40; k++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<DarkMatter2>(), 0f, 0f, 0);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 6f;
                Main.dust[dust].scale = (float)Main.rand.Next(100, 170) * 0.014f;
            }
            SoundEngine.PlaySound(SoundID.Item88, Projectile.Center);
        }
    }
}
