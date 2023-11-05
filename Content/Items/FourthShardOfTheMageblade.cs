using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class FourthShardOfTheMageblade : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Fourth Shard Of The Mageblade");
			// Tooltip.SetDefault("The Fourth and last piece of an ancient powerful blade...");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			
            
            
        }

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 1;
			Item.value = 2250;
			Item.rare = ItemRarityID.Cyan;	
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
                if (Main.LocalPlayer.HasItem(ModContent.ItemType<FirstShardOfTheMageblade>())
                    && Main.LocalPlayer.HasItem(ModContent.ItemType<SecondShardOfTheMageblade>())
                    && Main.LocalPlayer.HasItem(ModContent.ItemType<ThirdShardOfTheMageblade>())
                    && Main.LocalPlayer.HasItem(ModContent.ItemType<FourthShardOfTheMageblade>()))
                    spriteBatch.Draw(texture, position + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(255, 175, 247, 70), 0, origin, scale, SpriteEffects.None, 0);
            }

            return true;
        }

    }
}