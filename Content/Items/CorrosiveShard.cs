using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace PenumbraMod.Content.Items
{
	public class CorrosiveShard : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 9999;
			Item.value = 44250;
			Item.rare = ItemRarityID.Lime;	
		}
        float a = 1f;
        float progress;
        float r;
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Texture2D tex = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/JotapCosmicBullet").Value;

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
                spriteBatch.Draw(texture, position + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(0, 255, 0, 40), 0, origin, scale, SpriteEffects.None, 0);
            }
            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
            progress += 1f;
            r += 0.04f;
            if (progress >= 1 && progress <= 20)
            {
                a += 0.1f;
                if (a > 1f)
                    a = 1f;    
            }
            if (progress >= 40 && progress <= 60)
            {
                a -= 0.1f;
                if (a < 0f)
                    a = 0f;
            }
            if (progress > 70)
                progress = 0;

            Color baseColor = new Color(112, 239, 68, 0);
            Color lightColor = Item.GetAlpha(baseColor);
            baseColor.A = 0;
            float k = (lightColor.R / 255f + lightColor.G / 255f + lightColor.B / 255f) / 2f;
            spriteBatch.Draw(tex, position + new Vector2(4, -4), null, lightColor * k * a, r, tex.Size() / 2, scale * 0.4f, SpriteEffects.None, 0);

            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Lighting.AddLight(Item.Center, Color.Green.ToVector3() * 0.7f);
            // Draw the periodic glow effect behind the item when dropped in the world (hence PreDrawInWorld)
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Texture2D tex = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/JotapCosmicBullet").Value;

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

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(0, 255, 0, 40), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.20f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 6f).RotatedBy(radians) * time, frame, new Color(0, 255, 0, 40), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }
            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, drawPos, null, lightColor, 0, frameOrigin, scale, SpriteEffects.None, 0);
            progress += 1f;
            r += 0.05f;
            if (progress >= 1 && progress <= 20)
            {
                a += 0.1f;
                if (a > 1f)
                    a = 1f;
               
            }
            if (progress >= 40 && progress <= 60)
            {
                a -= 0.1f;
                if (a < 0f)
                    a = 0f;
            }
            if (progress > 70)
                progress = 0;

            Color baseColor = new Color(112, 239, 68, 0);
            Color lightColor2 = Item.GetAlpha(baseColor);
            baseColor.A = 0;
            float k = (lightColor2.R / 255f + lightColor2.G / 255f + lightColor2.B / 255f) / 2f;
            spriteBatch.Draw(tex, Item.position + new Vector2(19, 12) - Main.screenPosition, null, lightColor2 * k * a, r, tex.Size() / 2, scale * 0.4f, SpriteEffects.None, 0);
            return false;
        }
    }
}