using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Common;
using PenumbraMod.Content.Tiles;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class TheMageblade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Mageblade"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("[c/00ffff:An ancient sword used by an old hero...]" +
                "\n[c/00ffff:This blade is the powerful combination of the 4 legendary shards]" +
                "\n[c/00ffff:This hero was brave to save his world from the darkness and corruption]" +
                "\n[c/00ffff:Nobody knew his name, but they will remember who saved them from the evil...]" +
				"\nThrows a floating blade that follows the cursor and explodes in a bunch of parts."); */

        }

        public override void SetDefaults()
        {
            Item.damage = 250;
            Item.DamageType = DamageClass.Melee;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4;
            Item.value = 30450;
            Item.rare = ItemRarityID.Cyan;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TheMagebladeSwing>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.crit = 25;
            Item.noMelee = true;
            Item.channel = true;
        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int basic = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<TheMagebladeSwing>(), damage, knockback, player.whoAmI);
            Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
            return false;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.LightBlue.ToVector3() * 0.80f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }
    }
    public class PieceCutscene : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/FirstShardOfTheMageblade";
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            Color c = Color.White;
            return c * Projectile.Opacity;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Projectile.timeLeft = 1000000;
            Lighting.AddLight(Projectile.Center, Color.LightBlue.ToVector3() * 1f);
            // Some math magic to make it smoothly move up and down over time
            if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().MagebladeCutscene)
            {
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 120 && projOwner.GetModPlayer<PenumbraGlobalPlayer>().time <= 180)
                {
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = true;
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutepos = Projectile.Center;
                }

                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time < 400)
                {
                    const float TwoPi = (float)Math.PI * 4f;
                    float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 3f);
                    Projectile.Center = projOwner.Center + new Vector2(-150, -150f + offset);
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 401)
                {
                    Vector2 pos = projOwner.Top + new Vector2(0, -80);
                    Projectile.velocity = Projectile.DirectionTo(pos) * 1.5f;
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().KillFrags)
                    Projectile.Kill();
            }

        }
    }
    class PieceGlow : ModProjectile
    {
        public override string Texture => "PenumbraMod/Assets/Textures/MediumGlow-bigBG";
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(60, 233, 255, 0) * Projectile.Opacity;
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Projectile.timeLeft = 100000;
            // Some math magic to make it smoothly move up and down over time
            if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().MagebladeCutscene)
            {
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time < 400)
                {
                    const float TwoPi = (float)Math.PI * 4f;
                    float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 3f);
                    Projectile.Center = projOwner.Center + new Vector2(-150, -150f + offset);
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 401)
                {
                    Vector2 pos = projOwner.Top + new Vector2(0, -80);
                    Projectile.velocity = Projectile.DirectionTo(pos) * 1.5f;
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().KillFrags)
                    Projectile.Kill();
            }

        }
    }
    public class PieceCutscene2 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/SecondShardOfTheMageblade";
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            Color c = Color.White;
            return c * Projectile.Opacity;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Projectile.timeLeft = 100000;
            Lighting.AddLight(Projectile.Center, Color.LightBlue.ToVector3() * 1f);
            // Some math magic to make it smoothly move up and down over time
            if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().MagebladeCutscene)
            {
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 180 && projOwner.GetModPlayer<PenumbraGlobalPlayer>().time <= 240)
                {
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = true;
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutepos = Projectile.Center;
                }

                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time < 400)
                {
                    const float TwoPi = (float)Math.PI * 4f;
                    float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 3f);
                    Projectile.Center = projOwner.Center + new Vector2(-150, 100f + offset);
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 401)
                {
                    Vector2 pos = projOwner.Top + new Vector2(0, -80);
                    Projectile.velocity = Projectile.DirectionTo(pos) * 2;
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().KillFrags)
                    Projectile.Kill();
            }

        }
    }
    class PieceGlow2 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Assets/Textures/MediumGlow-bigBG";
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(60, 233, 255, 0) * Projectile.Opacity;
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Projectile.timeLeft = 1000000;
            // Some math magic to make it smoothly move up and down over time
            if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().MagebladeCutscene)
            {
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time < 400)
                {
                    const float TwoPi = (float)Math.PI * 4f;
                    float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 3f);
                    Projectile.Center = projOwner.Center + new Vector2(-150, 100f + offset);
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 401)
                {
                    Vector2 pos = projOwner.Top + new Vector2(0, -80);
                    Projectile.velocity = Projectile.DirectionTo(pos) * 2;
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().KillFrags)
                    Projectile.Kill();
            }
        }
    }
    public class PieceCutscene3 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/ThirdShardOfTheMageblade";
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            Color c = Color.White;
            return c * Projectile.Opacity;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Projectile.timeLeft = 100000;
            Lighting.AddLight(Projectile.Center, Color.LightBlue.ToVector3() * 1f);
            // Some math magic to make it smoothly move up and down over time
            if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().MagebladeCutscene)
            {
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 240 && projOwner.GetModPlayer<PenumbraGlobalPlayer>().time <= 300)
                {
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = true;
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutepos = Projectile.Center;
                }

                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time < 400)
                {
                    const float TwoPi = (float)Math.PI * 4f;
                    float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 3f);
                    Projectile.Center = projOwner.Center + new Vector2(150, -150f + offset);
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 401)
                {
                    Vector2 pos = projOwner.Top + new Vector2(0, -80);
                    Projectile.velocity = Projectile.DirectionTo(pos) * 1.5f;
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().KillFrags)
                    Projectile.Kill();
            }

        }
    }
    class PieceGlow3 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Assets/Textures/MediumGlow-bigBG";
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(60, 233, 255, 0) * Projectile.Opacity;
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Projectile.timeLeft = 100000;
            // Some math magic to make it smoothly move up and down over time
            if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().MagebladeCutscene)
            {
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time < 400)
                {
                    const float TwoPi = (float)Math.PI * 4f;
                    float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 3f);
                    Projectile.Center = projOwner.Center + new Vector2(150, -150f + offset);
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 401)
                {
                    Vector2 pos = projOwner.Top + new Vector2(0, -80);
                    Projectile.velocity = Projectile.DirectionTo(pos) * 1.5f;
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().KillFrags)
                    Projectile.Kill();
            }

        }
    }
    public class PieceCutscene4 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/FourthShardOfTheMageblade";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            Color c = Color.White;
            return c * Projectile.Opacity;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Projectile.timeLeft = 10;
            Lighting.AddLight(Projectile.Center, Color.LightBlue.ToVector3() * 1f);
            // Some math magic to make it smoothly move up and down over time
            if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().MagebladeCutscene)
            {
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 300 && projOwner.GetModPlayer<PenumbraGlobalPlayer>().time <= 360)
                {
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = true;
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutepos = Projectile.Center;
                }

                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time < 400)
                {
                    const float TwoPi = (float)Math.PI * 4f;
                    float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 3f);
                    Projectile.Center = projOwner.Center + new Vector2(150, 100f + offset);
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 401)
                {
                    Vector2 pos = projOwner.Top + new Vector2(0, -80);
                    Projectile.velocity = Projectile.DirectionTo(pos) * 2;
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = true;
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutepos = projOwner.Center;
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().KillFrags)
                {
                    Projectile.Kill();
                    Main.hideUI = false;
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = false;
                    AetheriumBlockModded.cutscene = false;
                    Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.getRect(), ModContent.ItemType<TheMageblade>());
                }
            }

        }
    }
    class PieceGlow4 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Assets/Textures/MediumGlow-bigBG";
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(60, 233, 255, 0) * Projectile.Opacity;
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Projectile.timeLeft = 10;
            Lighting.AddLight(Projectile.Center, Color.LightBlue.ToVector3() * 1f);
            // Some math magic to make it smoothly move up and down over time
            if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().MagebladeCutscene)
            {

                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time < 400)
                {
                    const float TwoPi = (float)Math.PI * 4f;
                    float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 3f);
                    Projectile.Center = projOwner.Center + new Vector2(150, 100f + offset);
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 401)
                {
                    Vector2 pos = projOwner.Top + new Vector2(0, -80);
                    Projectile.velocity = Projectile.DirectionTo(pos) * 2;
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = true;
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutepos = projOwner.Center;
                }
                if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().KillFrags)
                {
                    Projectile.Kill();
                    Main.hideUI = false;
                    projOwner.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = false;
                    AetheriumBlockModded.cutscene = false;
                }
            }

        }
    }
    public class PieceGlow5 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Assets/Textures/MediumGlow-bigBG";
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(255, 255, 255, 0) * Projectile.Opacity;
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Projectile.timeLeft = 10;
            Projectile.scale += 0.05f;
            if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().time >= 540)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha <= 0)
                    Projectile.alpha = 0;
            }
            if (projOwner.GetModPlayer<PenumbraGlobalPlayer>().KillFrags)
                    Projectile.Kill();
        }
    }
    public class PieceGlow6 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Assets/Textures/MediumGlow-bigBG";
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(255, 255, 255, 0) * Projectile.Opacity;
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.timeLeft = 10;
            Projectile.scale += 0.1f;
            Projectile.alpha += 10;
            if (Projectile.alpha >= 255)
                Projectile.Kill();

        }
    }
}