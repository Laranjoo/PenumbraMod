using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Dusts;
using System;
using System.Diagnostics.Metrics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class PhantomsPenumbraticDarkmatterScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Phantom's Penumbratic Darkmatter Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("[c/ad08cd:The scythe of Penumbra]" +
                "\n[c/75efff:This scythe makes enemies feel regret about their past...]" +
                "\n[c/75efff:What they did, how they did it, will be resolved now.]" +
                "\n[c/75efff:They won't like it]" +
                "\n[c/75efff:Swings and spins the scythe around the player]" +
                "\n[c/75efff:Shoots dark matter debris]" +
                "\n[c/75efff:They inflicts Dark matter]" +
                "\n[c/75efff:Hitting enemies also give Dark matter]" +
                "\n[c/75efff:Dark matter drains enemies souls...]" +
                "\n[c/e33ed7:Special ability:] [c/fff864:Throws the scythe and it follows the cursor, exploding dark matter debris and dealing massive damage to foes]" +
                "\n[c/ff0000:''It is strong enough for you?'']" +
                "\n[c/00FF00:-Developer Item-]"); */

        }

        public override void SetDefaults()
        {
            Item.damage = 585;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 98;
            Item.height = 92;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.knockBack = 2;
            Item.value = 453450;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PhantomsPenumbraticDarkmatterScytheSwing>();
            Item.shootSpeed = 2f;
            Item.noUseGraphic = false;
            Item.noMelee = true;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DarkMatter>(), 180);
        }
        bool notboollol = true;
        public int TwistedStyle = 0;
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
               
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noUseGraphic = true;
            }
            else
            {

                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = false;
            }
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[ModContent.ProjectileType<PhantomsPenumbraticDarkmatterScytheProj>()] < 1;
        }
        public sealed override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Mod == "Terraria" && line.Name == "ItemName")
            {
                Main.spriteBatch.End(); //end and begin main.spritebatch to apply a shader
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
                var lineshader = GameShaders.Misc["QueenSlime"];
                lineshader.Apply(null);
                Utils.DrawBorderString(Main.spriteBatch, line.Text, new Vector2(line.X, line.Y), Color.White, 1); //draw the tooltip manually
                Main.spriteBatch.End(); //then end and begin again to make remaining tooltip lines draw in the default way
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                return false;
            }
            return true;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.LightBlue.ToVector3() * 0.70f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Lighting.AddLight(Item.Center, Color.Purple.ToVector3() * 0.7f);
            // Draw the periodic glow effect behind the item when dropped in the world (hence PreDrawInWorld)
            Texture2D texture = TextureAssets.Item[Item.type].Value;

            Rectangle frame;

            if (Main.itemAnimations[Item.type] != null)
            {
                // In case this item is animated, this picks the correct frame
                frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);
            }
            else
            {
                frame = texture.Frame();
            }

            Vector2 frameOrigin = frame.Size() / 2f;
            Vector2 offset = new Vector2(Item.width / 2 - frameOrigin.X, Item.height - frame.Height);
            Vector2 drawPos = Item.position - Main.screenPosition + frameOrigin + offset;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;

            for (float i = 0f; i < 1f; i += 0.15f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(0, 255, 255, 40), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.20f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 6f).RotatedBy(radians) * time, frame, new Color(228, 87, 255, 55), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Item.noUseGraphic = true;
            if (TwistedStyle == 0)
            {
                if (player.velocity.Y > 5)
                {
                    if (player.direction == 0)
                        notboollol = true;
                    else
                        notboollol = false;
                }
                if (player.velocity.Y < -5)
                {
                    if (player.direction != 0)
                        notboollol = true;
                    else
                        notboollol = false;
                }

                int basic = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PhantomsPenumbraticDarkmatterScytheSwing>(), damage, knockback, player.whoAmI);
                if (notboollol == true)
                {
                    Main.projectile[basic].ai[1] = -1;
                    notboollol = false;
                }
                else
                {
                    Main.projectile[basic].ai[1] = 1;
                    notboollol = true;
                }
                Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
                TwistedStyle = 1;
                const int NumProjectiles = 3;
                for (int i = 0; i < NumProjectiles; i++)
                {
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(40));
                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 10f - Main.rand.NextFloat(0.1f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<PhantomDarkMatter>(), damage, knockback, player.whoAmI);

                }
                if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
                {
                    TwistedStyle = 2;
                    Item.useStyle = ItemUseStyleID.Shoot;
                    Item.noUseGraphic = true;
                }

            }

            else if (TwistedStyle == 1)
            {
                if (player.velocity.Y > 5)
                {
                    if (player.direction == 0)
                        notboollol = true;
                    else
                        notboollol = false;
                }
                if (player.velocity.Y < -5)
                {
                    if (player.direction != 0)
                        notboollol = true;
                    else
                        notboollol = false;
                }
                const int NumProjectiles4 = 8;
                for (int i = 0; i < NumProjectiles4; i++)
                {
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(360));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 10f - Main.rand.NextFloat(0.1f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<PhantomDarkMatter>(), damage, knockback, player.whoAmI);
                }
                int basic = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PhantomsPenumbraticDarkmatterScytheSwing2>(), damage, knockback, player.whoAmI);
                if (notboollol == true)
                {
                    Main.projectile[basic].ai[1] = -1;
                    notboollol = false;
                }
                else
                {
                    Main.projectile[basic].ai[1] = 1;
                    notboollol = true;
                }
                Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
                TwistedStyle = 0;

                if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
                {
                    TwistedStyle = 2;
                    Item.useStyle = ItemUseStyleID.Shoot;
                    Item.noUseGraphic = true;
                }
            }
            if (TwistedStyle == 2)
            {
                if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
                {
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(0));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 18f - Main.rand.NextFloat(0f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<PhantomsPenumbraticDarkmatterScytheProj>(), damage, knockback, player.whoAmI);

                    TwistedStyle = 0;
                    Item.useStyle = ItemUseStyleID.Shoot;
                    Item.noUseGraphic = true;
                }
            }
            return false;
        }
        // return false to stop vanilla from calling Projectile.NewProjectile.

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            if (Main.itemAnimations[Item.type] != null)
            {
                // In case this item is animated, this picks the correct frame
                frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[0]);
            }
            else
            {
                frame = texture.Frame();
            }
            float time = Main.GlobalTimeWrappedHourly;
            float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;
            for (float i = 0; i < 4; i += 0.35f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;
                spriteBatch.Draw(texture, position + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(93, 31, 216, 70), 0, origin, scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MirrorFragment>(), 20);
            recipe.AddIngredient(ModContent.ItemType<PenumbraticShard>(), 15);
            recipe.AddIngredient(ModContent.ItemType<DarkMatterItem>(), 12);
            recipe.AddIngredient(ItemID.LunarBar, 15);
            recipe.AddIngredient(ItemID.FragmentSolar, 15);
            recipe.AddIngredient(ItemID.SoulofMight, 15);
            recipe.AddIngredient(ItemID.SoulofNight, 15);
            recipe.AddIngredient(ItemID.GoldBar, 20);
            recipe.AddIngredient(ItemID.FallenStar, 15);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
        public class PhantomsPenumbraticDarkmatterScytheSwing : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Phantom's Penumbratic Darkmatter Scythe");
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

            }

            public override void SetDefaults()
            {
                Projectile.damage = 585;
                Projectile.width = 410;
                Projectile.height = 380;
                //Projectile.aiStyle = 1;
                // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
                Projectile.friendly = true;
                //projectile.magic = true;
                //projectile.extraUpdates = 100;
                Projectile.timeLeft = 20; // lowered from 300
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
                Projectile.netImportant = true;
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                target.AddBuff(ModContent.BuffType<DarkMatter>(), 180);
            }
            Vector2 dir = Vector2.Zero;
            Vector2 hlende = Vector2.Zero;
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
                        Main.EntitySpriteDraw(texture, lerpedPos, null, PenumbraMod.Phantom * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                    }
                }
                return true;
            }
            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
                float scaleFactor = 250f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
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
                Dust.NewDustPerfect(hitLineEnd, ModContent.DustType<DarkMatter2>(), Velocity: Vector2.Zero, 1, Color.Black, Scale: 2f);
                for (int x = 0; x < 10; x += 5)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Vector2 hitLineBT = Projectile.Center + rotationFactor.ToRotationVector2() * -x;
                        Dust.NewDustPerfect(hitLineBT, ModContent.DustType<DarkMatter2>(), Velocity: new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)));
                    }
                }
                // The following is for debugging the size of the hit line. This will allow you to easily see where it starts and ends. 
                // First check that our large rectangle intersects with the target hitbox.
                // Then we check to see if a line from the tip of the Jousting Lance to the "end" of the lance intersects with the target hitbox.
                if (/*lanceHitboxBounds.Intersects(targetHitbox)
                && */Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
                return false;
            }

            public override void AI()
            {
                // All Projectiles have timers that help to delay certain events
                // Projectile.ai[0], Projectile.ai[1] � timers that are automatically synchronized on the client and server
                // Projectile.localAI[0], Projectile.localAI[0] � only on the client
                // In this example, a timer is used to control the fade in / out and despawn of the Projectile
                //Projectile.ai[0] += 1f;
                Player player = Main.player[Projectile.owner];
                if (dir == Vector2.Zero)
                {
                    dir = Main.MouseWorld;
                    Projectile.rotation = (MathHelper.PiOver2 * Projectile.ai[1]) - MathHelper.PiOver4 + Projectile.DirectionTo(Main.MouseWorld).ToRotation();                 
                }
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 90);
                player.SetDummyItemTime(2);
                //FadeInAndOut();
                Projectile.Center = Main.player[Projectile.owner].Center;
                Projectile.ai[0] += 1f;
                Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((20 - Projectile.ai[0])));
               
                if (player.ownedProjectileCounts[ModContent.ProjectileType<PhantomsPenumbraticDarkmatterScytheProj>()] >= 1)
                {
                    Projectile.Kill();
                    Projectile.alpha = 255;
                    Projectile.active = false;
                }
                Projectile.ai[2]++;
                if (Projectile.ai[2] == 5 || Projectile.ai[2] == 8 || Projectile.ai[2] == 12 || Projectile.ai[2] == 15 || Projectile.ai[2] == 18)
                {
                    float rot = MathHelper.ToRadians(Main.rand.Next(0, 360));
                    Vector2 pos = new Vector2(190, 0).RotatedBy(rot);
                    int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + pos, pos / -14, ModContent.ProjectileType<PhantomDarkmatterEffect>(), 0, 0f, Main.player[Projectile.owner].whoAmI);
                }
            }
        }
        public class PhantomsPenumbraticDarkmatterScytheSwing2 : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Phantom's Penumbratic Darkmatter Scythe");
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            }

            public override void SetDefaults()
            {
                Projectile.damage = 585;
                Projectile.width = 410;
                Projectile.height = 380;
                //Projectile.aiStyle = 1;
                // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
                Projectile.friendly = true;
                //projectile.magic = true;
                //projectile.extraUpdates = 100;
                Projectile.timeLeft = 30; // lowered from 300
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
                Projectile.netImportant = true;
            }
            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                target.AddBuff(ModContent.BuffType<DarkMatter>(), 180);
            }
            Vector2 dir = Vector2.Zero;
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
                        Main.EntitySpriteDraw(texture, lerpedPos, null, PenumbraMod.Phantom * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                    }
                }
                return true;
            }
            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
                float scaleFactor = 250f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
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
                Dust.NewDustPerfect(hitLineEnd, ModContent.DustType<DarkMatter2>(), Velocity: Vector2.Zero, 1, Color.Black, Scale: 2f);
                for (int x = 0; x < 10; x += 5)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Vector2 hitLineBT = Projectile.Center + rotationFactor.ToRotationVector2() * -x;
                        Dust.NewDustPerfect(hitLineBT, ModContent.DustType<DarkMatter2>(), Velocity: new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)));
                    }
                }
                // The following is for debugging the size of the hit line. This will allow you to easily see where it starts and ends. 
                // First check that our large rectangle intersects with the target hitbox.
                // Then we check to see if a line from the tip of the Jousting Lance to the "end" of the lance intersects with the target hitbox.
                if (/*lanceHitboxBounds.Intersects(targetHitbox)
                && */Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
                return false;
            }
            public override void AI()
            {
                // All Projectiles have timers that help to delay certain events
                // Projectile.ai[0], Projectile.ai[1] � timers that are automatically synchronized on the client and server
                // Projectile.localAI[0], Projectile.localAI[0] � only on the client
                // In this example, a timer is used to control the fade in / out and despawn of the Projectile
                //Projectile.ai[0] += 1f;

                if (dir == Vector2.Zero)
                {
                    dir = Main.MouseWorld;
                    Projectile.rotation = (MathHelper.PiOver2 * Projectile.ai[1]) - MathHelper.PiOver4 + Projectile.DirectionTo(Main.MouseWorld).ToRotation();
                }
                //FadeInAndOut();
                Projectile.Center = Main.player[Projectile.owner].Center;
                Projectile.ai[0] += 1f;
                Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((40 - Projectile.ai[0])));
                Player player = Main.player[Projectile.owner];
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 90);
                player.SetDummyItemTime(2);
                if (player.ownedProjectileCounts[ModContent.ProjectileType<PhantomsPenumbraticDarkmatterScytheProj>()] >= 1)
                {
                    Projectile.Kill();
                    Projectile.alpha = 255;
                    Projectile.active = false;
                }
                Projectile.ai[2]++;
                if (Projectile.ai[2] == 5 || Projectile.ai[2] == 7 || Projectile.ai[2] == 10 || Projectile.ai[2] == 13 || Projectile.ai[2] == 17 || Projectile.ai[2] == 20 || Projectile.ai[2] == 23 || Projectile.ai[2] == 27)
                {
                    float rot = MathHelper.ToRadians(Main.rand.Next(0, 360));
                    Vector2 pos = new Vector2(190, 0).RotatedBy(rot);
                    int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + pos, pos / -14, ModContent.ProjectileType<PhantomDarkmatterEffect>(), 0, 0f, Main.player[Projectile.owner].whoAmI);
                }
            }
        }
    }
    public class PhantomsPenumbraticDarkmatterScytheProj : ModProjectile
    {
        public const int FadeInDuration = 100;
        public const int FadeOutDuration = 80;

        public const int TotalDuration = 180;

        public int Timer
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Phantom's Penumbratic Darkmatter Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 790;
            Projectile.width = 98;
            Projectile.height = 92;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1.2f;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DarkMatter>(), 180);
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0]  == 10 || Projectile.ai[0] == 20 || Projectile.ai[0] == 30 || Projectile.ai[0] == 40 || Projectile.ai[0] == 50 || Projectile.ai[0] == 60 || Projectile.ai[0] == 70 || Projectile.ai[0] == 80 || Projectile.ai[0] == 90
                || Projectile.ai[0] == 100 || Projectile.ai[0] == 110 || Projectile.ai[0] == 120 || Projectile.ai[0] == 130 || Projectile.ai[0] == 140 || Projectile.ai[0] == 150 || Projectile.ai[0] == 160 || Projectile.ai[0] == 170)
            {
                float rot = MathHelper.ToRadians(Main.rand.Next(0, 360));
                Vector2 pos = new Vector2(190, 0).RotatedBy(rot);
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + pos, pos / -14, ModContent.ProjectileType<PhantomDarkmatterEffect>(), 0, 0f, Main.player[Projectile.owner].whoAmI);
            }
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.PurpleTorch, 0f, 0f, 0);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 5.0f;
            Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.010f;
            int dust2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueCrystalShard, 0f, 0f, 0);
            Main.dust[dust2].noGravity = false;
            Main.dust[dust2].velocity *= 5.0f;
            Main.dust[dust2].scale = (float)Main.rand.Next(80, 140) * 0.010f;

            Projectile.velocity = (Main.MouseWorld - Projectile.Center) / 4;
            Lighting.AddLight(Projectile.Center, Color.LightBlue.ToVector3() * 0.93f);
            Projectile.scale *= 1.005f;
            Projectile.rotation += 0.4f;

            Timer += 1;
            if (Timer >= TotalDuration)
            {
                // Kill the projectile if it reaches it's intented lifetime
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
            Texture2D texture2 = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/PhantomsPenumbraticDarkmatterScytheProjEffect").Value;
            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
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
                    Main.EntitySpriteDraw(texture2, lerpedPos, null, PenumbraMod.Phantom * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 0);

                }
            }
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 50; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PurpleTorch, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 2.9f);
                Main.dust[dust].velocity *= 6.0f;
            }

            Vector2 launchVelocity = new Vector2(0, 0); // Create a velocity moving the left.
            for (int i = 0; i < 1; i++)
            {
                // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<DarkExplosion>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner, Projectile.scale *= 1.1f);
            }
            Vector2 launchVelocity2 = new Vector2(-12, 1); // Create a velocity moving the left.
            for (int i = 0; i < 15; i++)
            {
                // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                launchVelocity2 = launchVelocity2.RotatedBy(MathHelper.PiOver4);

                // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity2, ModContent.ProjectileType<PhantomDarkMatter>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item84, Projectile.position);
        }
       
    }
}