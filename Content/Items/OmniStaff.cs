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
	public class OmniStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
            /* Tooltip.SetDefault("Some of the gods most used weapon" +
                "\nFires a huge lightning that inflicts high voltage on enemies"); */
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 340;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.width = 126;
            Item.height = 124;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = 123000;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<Brightness10>();
            Item.shootSpeed = 2f;
        }
        int radius1 = 120;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ai = Main.rand.Next(100);
            Projectile t = Projectile.NewProjectileDirect(source, player.Center + player.DirectionTo(Main.MouseWorld) * 128, new Vector2(0, 16).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), ProjectileID.CultistBossLightningOrbArc, Item.damage, 0f, Main.myPlayer, player.DirectionTo(Main.MouseWorld).ToRotation(), ai);
            t.friendly = true;
            t.hostile = false;
            
            const int Repeats = 180;
            for (int i = 0; i < Repeats; ++i)
            {
                Vector2 position2 = player.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                int r = Dust.NewDust(position2, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity *= 0.9f;
                Main.dust[r].rotation += 1.1f;
            }
            return false; // return false to stop vanilla from calling Projectile.NewProjectile.
        }
        
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.LightBlue.ToVector3() * 0.80f * Main.essScale); // Makes this item glow when thrown out of inventory.
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
    }
}