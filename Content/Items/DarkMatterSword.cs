using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.Dusts;
using ReLogic.Content;
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
    public class DarkMatterSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dark Matter Blade");
            /* Tooltip.SetDefault("This dark sword drains the enemies souls" +
                "\nThrows the sword that follows the cursor, inflicting dark matter on foes" +
                "\n'Dark entities are watching...'"); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(3, 11));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 340;
            Item.DamageType = DamageClass.Melee;
            Item.width = 60;
            Item.height = 48;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4;
            Item.value = 42500;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DarkMatterSwordProj>();
            Item.shootSpeed = 8f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
        }

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[ModContent.ProjectileType<DarkMatterSwordProj>()] < 1 && player.ownedProjectileCounts[ModContent.ProjectileType<DarkGlowP>()] < 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DarkMatterItem>(), 15)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
    public class DarkMatterSwordProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dark Matter Sword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Main.projFrames[Projectile.type] = 11;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

        }

        public override void SetDefaults()
        {
            Projectile.damage = 435;
            Projectile.width = 60;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.scale = 1.3f;
            Projectile.hide = false;
            Projectile.alpha = 0;
            Projectile.extraUpdates = 2;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DarkMatter>(), 120);
        }
        int f;
        public override void AI()
        {
            f++;
            Projectile.ai[0] += 1f;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            Projectile.velocity = (Main.MouseWorld - Projectile.Center) / 8;
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            float projSpeed = 20;
            if (!player.channel)
            {
                Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;

                if (Projectile.Distance(player.Center) <= projSpeed)
                {
                    Projectile.Kill();
                    return;
                }
                return;
            }
            Projectile.timeLeft = 10;
            int dust = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<DarkMatter2>(), 0f, 0f, 0);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 3f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 170) * 0.012f;
            if (f == 1)
            {
                Vector2 velocity = Vector2.Zero;
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, velocity, ModContent.ProjectileType<DarkGlowP>(), 0, 0, player.whoAmI);

            }
            if (f == 21)
            {
                Vector2 velocity = (Main.MouseWorld - Projectile.Center) / 16f;
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, velocity, ModContent.ProjectileType<DarkRest>(), 0, 0, player.whoAmI);

            }
            if (f == 51)
            {
                Vector2 velocity2 = (Main.MouseWorld - Projectile.Center) / 16f;
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, velocity2, ModContent.ProjectileType<DarkRest2>(), 0, 0, player.whoAmI);

            }
            if (f == 90)
            {
                f = 3;

            }
            Lighting.AddLight(Projectile.Center, Color.Black.ToVector3() * 0.78f);
            Projectile.rotation += 0.1f;

        }
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
            float scaleFactor = 95f;
            float scaleFactor2 = 55f;// How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 23f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            // This Rectangle is the width and height of the Jousting Lance's hitbox which is used for the first step of collision.
            // You will need to modify the last two numbers if you have a bigger or smaller Jousting Lance.
            // Vanilla uses (0, 0, 300, 300) which that is quite large for the size of the Jousting Lance.
            // The size doesn't matter too much because this rectangle is only a basic check for the collision (the hit-line is much more important).
            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 300, 300);

            // Set the position of the large rectangle.
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;
            Vector2 hitLineEnd2 = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor2;

            // The following is for debugging the size of the hit line. This will allow you to easily see where it starts and ends.
            //Dust.NewDustPerfect(Projectile.Center, DustID.Pixie, Velocity: Vector2.Zero, 1);
            Dust.NewDustPerfect(hitLineEnd, ModContent.DustType<DarkMatter2>(), Velocity: Vector2.Zero, Scale: 0.6f);
            Dust.NewDustPerfect(hitLineEnd2, ModContent.DustType<DarkGlow>(), Velocity: Vector2.Zero, Scale: 0.8f);
            for (int x = 0; x < 10; x += 5)
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 hitLineBT = Projectile.Center + rotationFactor.ToRotationVector2() * -x;
                    Dust.NewDustPerfect(hitLineBT, ModContent.DustType<DarkMatter2>(), Velocity: new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)), Scale: 0.5f);
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
            return false;
        }
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
        public static Effect effect;
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            if (effect == null)
            {
                effect = ModContent.Request<Effect>("PenumbraMod/Effects/Shader", AssetRequestMode.ImmediateLoad).Value;
            }
            effect.Parameters["l"].SetValue(2.7f);
            effect.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

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



        public override void Kill(int timeLeft)
        {

            for (int k = 0; k < 50; k++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<DarkMatter2>(), 0f, 0f, 0);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 6f;
                Main.dust[dust].scale = (float)Main.rand.Next(100, 170) * 0.014f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item109, Projectile.position);
        }
    }
    public class DarkRest : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dark Matter Sword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

        }

        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.scale = 1.6f;
            Projectile.hide = false;
            Projectile.alpha = 0;
        }
        int f;
        public override void AI()
        {
            f++;
            Projectile.scale *= 0.94f;
            if (Projectile.scale < 0)
            {
                Projectile.Kill();
            }
            Projectile.rotation += 0.03f;
            if (f > 10)
            {
                Projectile.rotation *= 0.98f;
            }
            Projectile.velocity *= 0.95f;
            Lighting.AddLight(Projectile.Center, Color.Black.ToVector3() * 0.78f);

        }
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
            float scaleFactor = 95f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 23f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            // This Rectangle is the width and height of the Jousting Lance's hitbox which is used for the first step of collision.
            // You will need to modify the last two numbers if you have a bigger or smaller Jousting Lance.
            // Vanilla uses (0, 0, 300, 300) which that is quite large for the size of the Jousting Lance.
            // The size doesn't matter too much because this rectangle is only a basic check for the collision (the hit-line is much more important).
            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 300, 300);

            // Set the position of the large rectangle.
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;

            // The following is for debugging the size of the hit line. This will allow you to easily see where it starts and ends.
            //Dust.NewDustPerfect(Projectile.Center, DustID.Pixie, Velocity: Vector2.Zero, 1);
            Dust.NewDustPerfect(hitLineEnd, ModContent.DustType<DarkMatter2>(), Velocity: Vector2.Zero, 1);
            for (int x = 0; x < 20; x += 5)
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 hitLineBT = Projectile.Center + rotationFactor.ToRotationVector2() * -x;
                    Dust.NewDustPerfect(hitLineBT, ModContent.DustType<DarkMatter2>(), Velocity: new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)));
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
            return false;
        }

        public override void Kill(int timeLeft)
        {

            for (int k = 0; k < 10; k++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<DarkMatter2>(), 0f, 0f, 0);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 6f;
                Main.dust[dust].scale = (float)Main.rand.Next(100, 170) * 0.014f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
    }
    public class DarkRest2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dark Matter Sword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

        }

        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.scale = 1.6f;
            Projectile.hide = false;
            Projectile.alpha = 0;
        }
        int f;
        public override void AI()
        {
            f++;
            Projectile.scale *= 0.94f;
            if (Projectile.scale < 0)
            {
                Projectile.Kill();
            }
            Projectile.rotation += 0.1f;
            if (f > 10)
            {
                Projectile.rotation *= 0.98f;
            }
            Projectile.velocity *= 0.95f;
            Lighting.AddLight(Projectile.Center, Color.Black.ToVector3() * 0.78f);

        }
        Vector2 hlende = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
            float scaleFactor = 95f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 23f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

            // This Rectangle is the width and height of the Jousting Lance's hitbox which is used for the first step of collision.
            // You will need to modify the last two numbers if you have a bigger or smaller Jousting Lance.
            // Vanilla uses (0, 0, 300, 300) which that is quite large for the size of the Jousting Lance.
            // The size doesn't matter too much because this rectangle is only a basic check for the collision (the hit-line is much more important).
            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 300, 300);

            // Set the position of the large rectangle.
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;

            // The following is for debugging the size of the hit line. This will allow you to easily see where it starts and ends.
            //Dust.NewDustPerfect(Projectile.Center, DustID.Pixie, Velocity: Vector2.Zero, 1);
            Dust.NewDustPerfect(hitLineEnd, ModContent.DustType<DarkMatter2>(), Velocity: Vector2.Zero, 1);
            for (int x = 0; x < 20; x += 5)
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 hitLineBT = Projectile.Center + rotationFactor.ToRotationVector2() * -x;
                    Dust.NewDustPerfect(hitLineBT, ModContent.DustType<DarkMatter2>(), Velocity: new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)));
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
            return false;
        }

        public override void Kill(int timeLeft)
        {

            for (int k = 0; k < 10; k++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<DarkMatter2>(), 0f, 0f, 0);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 6f;
                Main.dust[dust].scale = (float)Main.rand.Next(100, 170) * 0.014f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
    }
    public class DarkGlowP : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dark Matter Sword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

        }

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            //Projectile.aiStyle = 1;
            // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
            Projectile.friendly = true;
            //projectile.magic = true;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;

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
        public override void AI()
        {
            Projectile.ai[0] += 1f;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            Projectile.velocity = (Main.MouseWorld - Projectile.Center) / 8;
            Projectile.timeLeft = 10;
            float projSpeed = 20;
            if (!player.channel)
            {
                Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
                if (Projectile.Distance(player.Center) <= projSpeed)
                {
                    Projectile.Kill();
                    return;
                }
                return;
            }
            Projectile.rotation += 0.1f;

        }
    }
}
