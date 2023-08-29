using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.SrplayerBomb
{
    public class SrplayertubesPenumbraticLunarBomb : ModItem
    {

        public override void SetDefaults()
        {
            Item.damage = 3000;
            Item.DamageType = DamageClass.Magic;
            Item.width = 32;
            Item.height = 22;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.mana = 50;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 50570;
            Item.rare = ItemRarityID.Purple;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<LunarBombProj>();
            Item.shootSpeed = 0f;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 3;
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
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MirrorFragment>(), 10);
            recipe.AddIngredient(ModContent.ItemType<PenumbraticShard>(), 8);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddIngredient(ItemID.FragmentNebula, 15);
            recipe.AddIngredient(ItemID.SoulofNight, 12);
            recipe.AddIngredient(ItemID.LivingFrostFireBlock, 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
    public class LunarBombProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 3000;
            Projectile.width = 30;
            Projectile.height = 22;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 175;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        float a = 1f;
        float b = 0.9f;
        float c = 0.01f;
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.78f);
            Projectile.ai[0] += 1f;
            if (++Projectile.frameCounter >= 11)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                  //  Main.NewText(Projectile.timeLeft.ToString());
                }
                   
            }
            a += 0.003f;
            b += 0.001f;
            c += 0.01f;
            Projectile.velocity.X *= 1f + Main.rand.Next(-3, 4) * c;
            Projectile.velocity.Y *= 1f + Main.rand.Next(-3, 4) * c;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Color c = lightColor;
            c.A = 0;
            int frameHeight = tex.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;
            Rectangle sourceRectangle = new(0, startY, tex.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float offsetX = 20f;
            origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;
            Vector2 drawOrigin = new(tex.Width * 0.5f, Projectile.height * 0.5f);
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, sourceRectangle, c, Projectile.rotation, drawOrigin, a, 0, 0);
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, sourceRectangle, c, Projectile.rotation, drawOrigin, b, 0, 0);

            return true;
        }
        public override void Kill(int timeLeft)
        {
            PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 12f, 6f, 20, 1000f, FullName);
            Main.instance.CameraModifiers.Add(modifier);
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(Projectile.position, 0, 0, DustID.BlueTorch, 1f, 0f, 0);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 8f;
                Main.dust[dust].scale = (float)Main.rand.Next(100, 170) * 0.010f;
            }

            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

            // TERRIBLE CODE BUT FUNCTIONAL

            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Explosion1>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ExplosionFX>(), 0, 8, Projectile.owner);

            // right
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(32, 2), new Vector2(0.001f, 0), ModContent.ProjectileType<Explosion2>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(32 * 2, 2), new Vector2(0.001f, 0), ModContent.ProjectileType<Explosion2>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(32 * 3, 2), new Vector2(0.001f, 0), ModContent.ProjectileType<Explosion2>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(32 * 4, 2), new Vector2(0.001f, 0), ModContent.ProjectileType<Explosion2>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(32 * 5, 2), new Vector2(0.001f, 0), ModContent.ProjectileType<Explosion2>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(32 * 6, 2), new Vector2(0.001f, 0), ModContent.ProjectileType<Explosion3>(), Projectile.damage, 8, Projectile.owner);

            //left
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(-32, -2), new Vector2(-0.001f, 0), ModContent.ProjectileType<Explosion2>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(-32 * 2, -2), new Vector2(-0.001f, 0), ModContent.ProjectileType<Explosion2>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(-32 * 3, -2), new Vector2(-0.001f, 0), ModContent.ProjectileType<Explosion2>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(-32 * 4, -2), new Vector2(-0.001f, 0), ModContent.ProjectileType<Explosion2>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(-32 * 5, -2), new Vector2(-0.001f, 0), ModContent.ProjectileType<Explosion2>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(-32 * 6, -2), new Vector2(-0.001f, 0), ModContent.ProjectileType<Explosion3>(), Projectile.damage, 8, Projectile.owner);

            //up
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(0, -32), new Vector2(0, -0.001f), ModContent.ProjectileType<Explosion2up>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(0, -32 * 2), new Vector2(0, -0.001f), ModContent.ProjectileType<Explosion2up>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(0, -32 * 3), new Vector2(0, -0.001f), ModContent.ProjectileType<Explosion2up>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(0, -32 * 4), new Vector2(0, -0.001f), ModContent.ProjectileType<Explosion2up>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(0, -32 * 5), new Vector2(0, -0.001f), ModContent.ProjectileType<Explosion2up>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(0, -32 * 6), new Vector2(0, -0.001f), ModContent.ProjectileType<Explosion3up>(), Projectile.damage, 8, Projectile.owner);

            //down
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(0, 32), new Vector2(0, 0.001f), ModContent.ProjectileType<Explosion2up>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(0, 32 * 2), new Vector2(0, 0.001f), ModContent.ProjectileType<Explosion2up>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(0, 32 * 3), new Vector2(0, 0.001f), ModContent.ProjectileType<Explosion2up>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(0, 32 * 4), new Vector2(0, 0.001f), ModContent.ProjectileType<Explosion2up>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(0, 32 * 5), new Vector2(0, 0.01f), ModContent.ProjectileType<Explosion2up>(), Projectile.damage, 8, Projectile.owner);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(-4, 32 * 6), new Vector2(0, 0.001f), ModContent.ProjectileType<Explosion3down>(), Projectile.damage, 8, Projectile.owner);
        }
    }
    public class Explosion1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 3000;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }


        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.78f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.Kill();
                    //  Main.NewText(Projectile.timeLeft.ToString());
                }

            }

        }
    }
    public class Explosion2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 3000;
            Projectile.width = 32;
            Projectile.height = 28;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }


        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.78f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.Kill();
                    //  Main.NewText(Projectile.timeLeft.ToString());
                }

            }

        }
    }
    public class Explosion2up : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 3000;
            Projectile.width = 28;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }


        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.78f);
            Projectile.rotation = MathHelper.PiOver2;
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.Kill();
                    //  Main.NewText(Projectile.timeLeft.ToString());
                }

            }
        }
    }
    public class Explosion3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 3000;
            Projectile.width = 32;
            Projectile.height = 28;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }


        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.78f);
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.Kill();
                    //  Main.NewText(Projectile.timeLeft.ToString());
                }

            }

        }
    }
    public class Explosion3up : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/SrplayerBomb/Explosion3";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 3000;
            Projectile.width = 28;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }


        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.78f);
            Projectile.rotation = -MathHelper.PiOver2;
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.Kill();
                    //  Main.NewText(Projectile.timeLeft.ToString());
                }

            }
        }
    }
    public class Explosion3down : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/SrplayerBomb/Explosion3";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 3000;
            Projectile.width = 28;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }


        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.78f);
            Projectile.rotation = MathHelper.PiOver2;
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.Kill();
                    //  Main.NewText(Projectile.timeLeft.ToString());
                }

            }
        }
    }
    public class ExplosionFX : ModProjectile
    {
        public override string Texture => "PenumbraMod/Assets/Textures/FireTextures/FireTexture-BitSmallBG";
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
            Projectile.alpha = 50;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(94, 175, 255, 0) * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.alpha += 10;
            if (Projectile.alpha > 255)
                Projectile.Kill();
            Projectile.rotation += 0.4f;
            Projectile.scale += 0.06f;
        }
    }
}