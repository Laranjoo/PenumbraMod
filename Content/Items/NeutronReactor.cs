using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content;
using PenumbraMod.Content.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class NeutronReactor : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nature Grass"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("From the pure nature..." +
				"\nFires a Nature Beam that pushes enemies far"); */

		}

		public override void SetDefaults()
		{
			Item.damage = 270;
			Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
			Item.width = 26;
			Item.height = 26;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 16f;
			Item.value = 1000;
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<NeutronReactorRay>();
			Item.shootSpeed = 18f;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile t = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<NeutronReactorRay>(), damage, knockback, player.whoAmI);
            t.ai[2]++;
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<NeutronReactorHeld>(), 0, 0, player.whoAmI);       
            return false;
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
        float a = 1f;
        float progress;
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Texture2D tex = ModContent.Request<Texture2D>("PenumbraMod/Content/ExpertAccessorySlot/LightUnlocked").Value;
           
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
            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
            if (Main.LocalPlayer.HeldItem.type == Item.type)
            {
                progress += 1f;
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

                Color baseColor = new Color(238, 0, 255, 0);
                Color lightColor = Item.GetAlpha(baseColor);
                baseColor.A = 0;
                float k = (lightColor.R / 255f + lightColor.G / 255f + lightColor.B / 255f) / 2f;
                spriteBatch.Draw(tex, position, null, lightColor * k * a, 0, tex.Size() / 2, scale * 1.2f, SpriteEffects.None, 0);
            }
            else
            {
                a -= 0.1f;
                if (a < 0f)
                    a = 0f;
            }

            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MirrorFragment>(), 10);
            recipe.AddIngredient(ModContent.ItemType<PenumbraticShard>(), 5);
            recipe.AddIngredient(ItemID.LunarBar, 8);
            recipe.AddIngredient(ItemID.FragmentNebula, 10);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}